using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;
using Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;
using Microservicios.Atracciones.Catalog.Business.Exceptions;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.DataAccess.Common;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Models;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;

namespace Microservicios.Atracciones.Catalog.Business.Services;

public class AttractionService : IAttractionService
{
    private readonly IAttractionDataService _attractionData;
    private readonly IInventoryDataService _inventoryData;
    private readonly IUnitOfWork _uow;

    public AttractionService(IAttractionDataService attractionData, IInventoryDataService inventoryData, IUnitOfWork uow)
    {
        _attractionData = attractionData;
        _inventoryData = inventoryData;
        _uow = uow;
    }

    public async Task<PagedResult<AttractionSummaryResponse>> SearchAsync(AttractionSearchRequest request)
    {
        var filters = new AttractionQueryFilters
        {
            SearchTerm = request.SearchTerm ?? string.Empty,
            LocationId = request.LocationId,
            CategorySlug = request.CategorySlug,
            SubcategoryId = request.SubcategoryId,
            TagId = request.TagId,
            TagIds = request.TagIds,
            LanguageId = request.LanguageId,
            LanguageIds = request.LanguageIds,
            MinRating = request.MinRating,
            DifficultyLevel = request.DifficultyLevel,
            DifficultyLevels = request.DifficultyLevels,
            MinPrice = request.MinPrice,
            MaxPrice = request.MaxPrice,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            IsPublished = true,
            IsActive = true
        };

        var paged = await _attractionData.SearchAttractionsAsync(filters);

        return new PagedResult<AttractionSummaryResponse>
        {
            Items = paged.Items.Select(MapToSummary).ToList(),
            TotalCount = paged.TotalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize
        };
    }

    public async Task<PagedResult<AttractionSummaryResponse>> SearchManagementAsync(AttractionSearchRequest request, Guid currentUserId, bool isAdmin)
    {
        var filters = new AttractionQueryFilters
        {
            SearchTerm = request.SearchTerm ?? string.Empty,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            IsPublished = null
        };

        if (!isAdmin)
        {
            filters.ManagedById = currentUserId;
        }

        var paged = await _attractionData.SearchAttractionsAsync(filters);

        return new PagedResult<AttractionSummaryResponse>
        {
            Items = paged.Items.Select(MapToSummary).ToList(),
            TotalCount = paged.TotalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize
        };
    }

    public async Task<AttractionDetailResponse?> GetDetailBySlugAsync(string slug, short? languageId = null)
    {
        var node = await _attractionData.GetAttractionBySlugAsync(slug, languageId);
        if (node == null) return null;

        var products = await _inventoryData.GetProductsAsync(node.Id, languageId);
        var itinerary = (await GetItinerariesAsync(node.Id)).FirstOrDefault();

        return MapToDetail(node, products, itinerary);
    }

    public async Task<IEnumerable<AttractionSummaryResponse>> GetTopRatedAsync(int count = 6)
    {
        var nodes = await _attractionData.GetTopRatedAsync(count);
        return nodes.Select(MapToSummary);
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync(Guid attractionId, short? languageId = null)
    {
        var products = await _inventoryData.GetProductsAsync(attractionId, languageId);
        return products.Select(MapToProductResponse);
    }

    public async Task<Guid> CreateAsync(CreateAttractionRequest request, Guid userId, bool isAdmin)
    {
        var attraction = new Attraction
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            LocationId = request.LocationId,
            SubcategoryId = request.SubcategoryId,
            DescriptionShort = request.DescriptionShort,
            DescriptionFull = request.DescriptionFull,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MeetingPoint = request.MeetingPoint,
            DifficultyLevel = request.DifficultyLevel,
            ManagedById = isAdmin ? null : userId,
            Slug = GenerateSlug(request.Name)
        };

        return await _attractionData.AddAttractionAsync(attraction);
    }

    public async Task<Guid> CreateCompleteAsync(CreateCompleteAttractionRequest request, Guid userId, bool isAdmin)
    {
        var attraction = new Attraction
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            LocationId = request.LocationId,
            SubcategoryId = request.SubcategoryId,
            DescriptionShort = request.DescriptionShort,
            DescriptionFull = request.DescriptionFull,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MeetingPoint = request.MeetingPoint,
            DifficultyLevel = request.DifficultyLevel,
            ManagedById = isAdmin ? null : userId,
            IsPublished = false,
            Slug = GenerateSlug(request.Name)
        };

        foreach (var t in request.Translations)
        {
            attraction.Translations.Add(new AttractionTranslation
            {
                LanguageId = t.LanguageId,
                Name = t.Name,
                DescriptionShort = t.DescriptionShort,
                DescriptionFull = t.DescriptionFull,
                MeetingPoint = t.MeetingPoint
            });
        }

        foreach (var gl in request.GuideLanguages)
        {
            attraction.Languages.Add(new AttractionLanguage
            {
                LanguageId = gl.LanguageId,
                GuideType = gl.GuideType
            });
        }

        foreach (var m in request.Media)
        {
            attraction.Media.Add(new AttractionMedia
            {
                MediaTypeId = m.MediaTypeId,
                Url = m.Url,
                Title = m.Title,
                IsMain = m.IsMain,
                SortOrder = m.SortOrder
            });
        }

        foreach (var tagId in request.Tags)
        {
            attraction.Tags.Add(new AttractionTag { TagId = tagId });
        }

        foreach (var inc in request.Inclusions)
        {
            attraction.Inclusions.Add(new AttractionInclusion 
            { 
                InclusionItemId = inc.InclusionItemId,
                Type = inc.Type
            });
        }

        if (!request.Products.Any())
            throw new ValidationException("Debe agregar al menos una modalidad (producto) a la atracciÃ³n.");

        foreach (var p in request.Products)
        {
            if (!p.PriceTiers.Any())
                throw new ValidationException($"La modalidad '{p.Title}' debe tener al menos una categorÃ­a de ticket (Price Tier) asignada.");

            var product = new ProductOption
            {
                Id = Guid.NewGuid(),
                AttractionId = attraction.Id,
                Title = p.Title,
                Description = p.Description,
                DurationMinutes = p.DurationMinutes,
                DurationDescription = p.DurationDescription,
                CancelPolicyHours = p.CancelPolicyHours,
                CancelPolicyText = p.CancelPolicyText,
                MaxGroupSize = p.MaxGroupSize,
                MinParticipants = p.MinParticipants,
                IsPrivate = p.IsPrivate,
                Slug = GenerateSlug(p.Title),
                IsActive = true
            };

            foreach (var pt in p.PriceTiers)
            {
                product.PriceTiers.Add(new PriceTier
                {
                    TicketCategoryId = pt.TicketCategoryId,
                    Price = pt.Price,
                    CurrencyCode = pt.CurrencyCode,
                    IsActive = true
                });
            }

            attraction.ProductOptions.Add(product);
        }

        if (request.Itinerary != null)
        {
            var itinerary = new TourItinerary
            {
                Id = Guid.NewGuid(),
                AttractionId = attraction.Id,
                LanguageId = request.Itinerary.LanguageId > 0 ? request.Itinerary.LanguageId : (short)1,
                Overview = request.Itinerary.Overview,
                Title = attraction.Name
            };

            foreach (var s in request.Itinerary.Stops)
            {
                itinerary.Stops.Add(new TourStop
                {
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    StopNumber = s.StopNumber,
                    AdmissionType = s.AdmissionType,
                    DurationMinutes = s.StayTimeMinutes
                });
            }

            attraction.Itineraries.Add(itinerary);
        }

        await _uow.Attractions.AddAsync(attraction);
        await _uow.CompleteAsync();

        return attraction.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateAttractionRequest request, Guid userId, bool isAdmin)
    {
        var existing = await _attractionData.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException("AtracciÃ³n", id);

        if (!isAdmin && existing.ManagedById != userId)
            throw new UnauthorizedBusinessException("No tienes permiso para editar esta atracciÃ³n.");

        var updated = new Attraction
        {
            Id = id,
            Name = request.Name,
            LocationId = request.LocationId,
            SubcategoryId = request.SubcategoryId,
            DescriptionShort = request.DescriptionShort,
            DescriptionFull = request.DescriptionFull,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MeetingPoint = request.MeetingPoint,
            DifficultyLevel = request.DifficultyLevel,
            IsPublished = existing.IsPublished,
        };

        return await _attractionData.UpdateAsync(updated);
    }

    public async Task<bool> UpdateCompleteAsync(Guid id, CreateCompleteAttractionRequest request, Guid userId, bool isAdmin)
    {
        var existing = await _attractionData.GetFullByIdAsync(id);
        if (existing == null)
            throw new NotFoundException("Atracción", id);

        if (!isAdmin && existing.ManagedById != userId)
            throw new UnauthorizedBusinessException("No tienes permiso para editar esta atracción.");

        // 1. Actualizar campos básicos
        existing.Name = request.Name;
        existing.LocationId = request.LocationId;
        existing.SubcategoryId = request.SubcategoryId;
        existing.DescriptionShort = request.DescriptionShort;
        existing.DescriptionFull = request.DescriptionFull;
        existing.Address = request.Address;
        existing.Latitude = request.Latitude;
        existing.Longitude = request.Longitude;
        existing.MeetingPoint = request.MeetingPoint;
        existing.DifficultyLevel = request.DifficultyLevel;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.Slug = GenerateSlug(request.Name);

        // 2. Actualizar traducciones
        existing.Translations.Clear();
        foreach (var t in request.Translations)
        {
            existing.Translations.Add(new AttractionTranslation
            {
                LanguageId = t.LanguageId,
                Name = t.Name,
                DescriptionShort = t.DescriptionShort,
                DescriptionFull = t.DescriptionFull,
                MeetingPoint = t.MeetingPoint
            });
        }

        // 3. Actualizar idiomas de guía
        existing.Languages.Clear();
        foreach (var gl in request.GuideLanguages)
        {
            existing.Languages.Add(new AttractionLanguage
            {
                LanguageId = gl.LanguageId,
                GuideType = gl.GuideType
            });
        }

        // 4. Actualizar galería de imágenes/videos
        existing.Media.Clear();
        foreach (var m in request.Media)
        {
            existing.Media.Add(new AttractionMedia
            {
                MediaTypeId = m.MediaTypeId,
                Url = m.Url,
                Title = m.Title,
                IsMain = m.IsMain,
                SortOrder = m.SortOrder
            });
        }

        // 5. Actualizar etiquetas (Tags)
        existing.Tags.Clear();
        foreach (var tagId in request.Tags)
        {
            existing.Tags.Add(new AttractionTag { TagId = tagId });
        }

        // 6. Actualizar inclusiones/exclusiones
        existing.Inclusions.Clear();
        foreach (var inc in request.Inclusions)
        {
            existing.Inclusions.Add(new AttractionInclusion
            {
                InclusionItemId = inc.InclusionItemId,
                Type = inc.Type
            });
        }

        // 7. Actualizar modalidades de producto (ProductOptions)
        existing.ProductOptions.Clear();
        if (!request.Products.Any())
            throw new ValidationException("Debe agregar al menos una modalidad (producto) a la atracción.");

        foreach (var p in request.Products)
        {
            if (!p.PriceTiers.Any())
                throw new ValidationException($"La modalidad '{p.Title}' debe tener al menos una categoría de ticket (Price Tier) asignada.");

            var product = new ProductOption
            {
                Id = Guid.NewGuid(),
                AttractionId = existing.Id,
                Title = p.Title,
                Description = p.Description,
                DurationMinutes = p.DurationMinutes,
                DurationDescription = p.DurationDescription,
                CancelPolicyHours = p.CancelPolicyHours,
                CancelPolicyText = p.CancelPolicyText,
                MaxGroupSize = p.MaxGroupSize,
                MinParticipants = p.MinParticipants,
                IsPrivate = p.IsPrivate,
                Slug = GenerateSlug(p.Title),
                IsActive = true
            };

            foreach (var pt in p.PriceTiers)
            {
                product.PriceTiers.Add(new PriceTier
                {
                    TicketCategoryId = pt.TicketCategoryId,
                    Price = pt.Price,
                    CurrencyCode = pt.CurrencyCode,
                    IsActive = true
                });
            }

            existing.ProductOptions.Add(product);
        }

        // 8. Actualizar itinerario
        existing.Itineraries.Clear();
        if (request.Itinerary != null)
        {
            var itinerary = new TourItinerary
            {
                Id = Guid.NewGuid(),
                AttractionId = existing.Id,
                LanguageId = request.Itinerary.LanguageId > 0 ? request.Itinerary.LanguageId : (short)1,
                Overview = request.Itinerary.Overview,
                Title = existing.Name
            };

            foreach (var s in request.Itinerary.Stops)
            {
                itinerary.Stops.Add(new TourStop
                {
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    StopNumber = s.StopNumber,
                    AdmissionType = s.AdmissionType,
                    DurationMinutes = s.StayTimeMinutes
                });
            }

            existing.Itineraries.Add(itinerary);
        }

        await _uow.CompleteAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId, bool isAdmin)
    {
        var existing = await _attractionData.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException("AtracciÃ³n", id);

        if (!isAdmin)
            throw new UnauthorizedBusinessException("Solo los administradores pueden eliminar atracciones.");

        return await _attractionData.DeleteAsync(id);
    }

    public async Task<bool> ToggleStatusAsync(Guid id, bool isPublished, Guid userId, bool isAdmin)
    {
        var attraction = await _uow.Attractions.GetByIdAsync(id);
        if (attraction == null) return false;

        if (!isAdmin && attraction.ManagedById != userId) return false;

        attraction.IsPublished = isPublished;
        attraction.UpdatedAt = DateTime.UtcNow;

        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> ToggleActiveAsync(Guid id, bool isActive, Guid userId, bool isAdmin)
    {
        var attraction = await _uow.Attractions.GetByIdAsync(id);
        if (attraction == null) return false;

        if (!isAdmin && attraction.ManagedById != userId) return false;

        attraction.IsActive = isActive;
        attraction.UpdatedAt = DateTime.UtcNow;

        return await _uow.CompleteAsync() > 0;
    }

    public async Task<AttractionFullEditionResponse?> GetCompleteByIdAsync(Guid id, Guid userId, bool isAdmin)
    {
        var a = await _attractionData.GetFullByIdAsync(id);
        if (a == null) return null;

        if (!isAdmin && a.ManagedById != userId)
            throw new UnauthorizedBusinessException("No tienes permiso para ver esta atracciÃ³n.");

        Guid? stateId = null;
        Guid? countryId = null;

        if (a.Location != null)
        {
            if (a.Location.Type == "city")
            {
                stateId = a.Location.ParentId;
                countryId = a.Location.Parent?.ParentId;
            }
            else if (a.Location.Type == "state")
            {
                stateId = a.Location.Id;
                countryId = a.Location.ParentId;
            }
            else if (a.Location.Type == "country")
            {
                countryId = a.Location.Id;
            }
        }

        var response = new AttractionFullEditionResponse
        {
            Id = a.Id,
            Slug = a.Slug,
            Name = a.Name,
            DescriptionShort = a.DescriptionShort,
            DescriptionFull = a.DescriptionFull,
            RatingAverage = a.RatingAverage,
            RatingCount = a.RatingCount,
            DifficultyLevel = a.DifficultyLevel,
            Address = a.Address,
            MeetingPoint = a.MeetingPoint,
            Latitude = a.Latitude,
            Longitude = a.Longitude,
            LocationId = a.LocationId,
            LocationName = a.Location?.Name ?? "",
            LocationCountryCode = a.Location?.CountryCode ?? "",
            CategoryName = a.Subcategory?.Category?.Name ?? "",
            SubcategoryName = a.Subcategory?.Name ?? "",
            StateId = stateId ?? Guid.Empty,
            CountryId = countryId ?? Guid.Empty,
            IsActive = a.IsActive,
            IsPublished = a.IsPublished,
            Gallery = a.Media.Select(m => new MediaResponse
            {
                Url = m.Url,
                Title = m.Title,
                IsMain = m.IsMain,
                SortOrder = m.SortOrder
            }).ToList(),
            Tags = a.Tags.Select(at => new TagResponse { Id = at.TagId, Name = at.Tag?.Name ?? "" }).ToList(),
            Inclusions = a.Inclusions.Select(ai => new InclusionResponse
            {
                Id = ai.InclusionItemId,
                InclusionItemId = ai.InclusionItemId,
                Name = ai.InclusionItem?.DefaultText ?? "",
                Type = ai.Type
            }).ToList(),
            GuideLanguages = a.Languages.Select(al => new GuideLanguageResponse
            {
                LanguageId = al.LanguageId,
                Name = al.LanguageId switch { 1 => "EspaÃ±ol", 2 => "InglÃ©s", _ => $"Idioma {al.LanguageId}" },
                GuideType = al.GuideType
            }).ToList(),
            Products = a.ProductOptions.Select(po => new ProductResponse
            {
                Id = po.Id,
                Title = po.Title,
                Description = po.Description,
                DurationMinutes = po.DurationMinutes,
                DurationDescription = po.DurationDescription,
                CancelPolicyHours = po.CancelPolicyHours,
                MaxGroupSize = po.MaxGroupSize,
                MinParticipants = po.MinParticipants,
                IsPrivate = po.IsPrivate,
                PriceTiers = po.PriceTiers.Select(pt => new PriceTierResponse
                {
                    Id = pt.Id,
                    TicketCategoryId = pt.TicketCategoryId,
                    Price = pt.Price,
                    CurrencyCode = pt.CurrencyCode
                }).ToList()
            }).ToList()
        };

        var itinerary = a.Itineraries.FirstOrDefault();
        if (itinerary != null)
        {
            response.Itinerary = new ItineraryResponse
            {
                Id = itinerary.Id,
                LanguageId = itinerary.LanguageId,
                Title = itinerary.Title,
                Description = itinerary.Overview,
                Stops = itinerary.Stops.OrderBy(s => s.StopNumber).Select(s => new TourStopResponse
                {
                    Id = s.Id,
                    StopNumber = s.StopNumber,
                    Name = s.Name,
                    Description = s.Description,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    AdmissionType = s.AdmissionType,
                    DurationMinutes = s.DurationMinutes
                }).ToList()
            };
        }

        return response;
    }

    public async Task<IEnumerable<ItineraryResponse>> GetItinerariesAsync(Guid attractionId)
    {
        var itineraries = await _uow.TourItineraries.Query()
            .Where(ti => ti.AttractionId == attractionId)
            .Include(ti => ti.Stops)
            .ToListAsync();

        return itineraries.Select(ti => new ItineraryResponse
        {
            Id = ti.Id,
            LanguageId = ti.LanguageId,
            Title = ti.Title,
            Description = ti.Overview,
            TotalDistanceKm = ti.TotalDistanceKm,
            Stops = ti.Stops.OrderBy(s => s.StopNumber).Select(s => new TourStopResponse
            {
                Id = s.Id,
                StopNumber = s.StopNumber,
                Name = s.Name,
                Description = s.Description,
                DurationMinutes = s.DurationMinutes,
                AdmissionType = s.AdmissionType,
                Latitude = s.Latitude,
                Longitude = s.Longitude
            }).ToList()
        });
    }

    public async Task<Guid> CreateItineraryAsync(Guid attractionId, CreateItineraryRequest request)
    {
        var itinerary = new TourItinerary
        {
            Id = Guid.NewGuid(),
            AttractionId = attractionId,
            LanguageId = request.LanguageId,
            Title = request.Title,
            Overview = request.Description,
            TotalDistanceKm = request.TotalDistanceKm,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.TourItineraries.AddAsync(itinerary);
        await _uow.CompleteAsync();
        return itinerary.Id;
    }

    public async Task<Guid> AddStopAsync(Guid itineraryId, CreateTourStopRequest request)
    {
        var stop = new TourStop
        {
            Id = Guid.NewGuid(),
            ItineraryId = itineraryId,
            StopNumber = request.StopNumber,
            Name = request.Name,
            Description = request.Description,
            DurationMinutes = (short?)request.DurationMinutes,
            AdmissionType = request.AdmissionType,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        await _uow.TourStops.AddAsync(stop);
        await _uow.CompleteAsync();
        return stop.Id;
    }

    public async Task<bool> DeleteItineraryAsync(Guid id)
    {
        var itinerary = await _uow.TourItineraries.GetByIdAsync(id);
        if (itinerary == null) return false;
        _uow.TourItineraries.Delete(itinerary);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteStopAsync(Guid id)
    {
        var stop = await _uow.TourStops.GetByIdAsync(id);
        if (stop == null) return false;
        _uow.TourStops.Delete(stop);
        return await _uow.CompleteAsync() > 0;
    }

    private static string GenerateSlug(string name)
    {
        return name.ToLower()
                   .Replace(" ", "-")
                   .Replace("Ã¡", "a")
                   .Replace("Ã©", "e")
                   .Replace("Ã­", "i")
                   .Replace("Ã³", "o")
                   .Replace("Ãº", "u")
                   .Replace("Ã±", "n")
                   + "-" + Guid.NewGuid().ToString().Substring(0, 4);
    }

    private static AttractionSummaryResponse MapToSummary(AttractionNode node)
    {
        var mainImage = node.MediaGallery.FirstOrDefault(m => m.IsMain)?.Url
                     ?? node.MediaGallery.FirstOrDefault()?.Url;

        return new AttractionSummaryResponse
        {
            Id = node.Id,
            Slug = node.Slug,
            Name = node.Name,
            DescriptionShort = node.DescriptionShort,
            LocationName = node.LocationName,
            LocationCountryCode = node.LocationCountryCode,
            CategoryName = node.CategoryName,
            SubcategoryName = node.SubcategoryName,
            RatingAverage = node.RatingAverage,
            RatingCount = node.RatingCount,
            DifficultyLevel = node.DifficultyLevel,
            MainImageUrl = mainImage,
            StartingPrice = node.StartingPrice,
            IsActive = node.IsActive,
            IsPublished = node.IsPublished,
            ModalityCount = node.ModalityCount
        };
    }

    private static AttractionDetailResponse MapToDetail(AttractionNode node, IEnumerable<ProductNode> products, ItineraryResponse? itinerary = null)
    {
        return new AttractionDetailResponse
        {
            Id = node.Id,
            Slug = node.Slug,
            Name = node.Name,
            DescriptionShort = node.DescriptionShort,
            DescriptionFull = node.DescriptionFull,
            RatingAverage = node.RatingAverage,
            RatingCount = node.RatingCount,
            DifficultyLevel = node.DifficultyLevel,
            Address = node.Address,
            MeetingPoint = node.MeetingPoint,
            Latitude = node.Latitude,
            Longitude = node.Longitude,
            LocationId = node.LocationId,
            LocationName = node.LocationName,
            LocationCountryCode = node.LocationCountryCode,
            CategoryName = node.CategoryName,
            SubcategoryName = node.SubcategoryName,
            Gallery = node.MediaGallery.Select(m => new MediaResponse
            {
                Url = m.Url,
                Title = m.Title,
                IsMain = m.IsMain,
                SortOrder = m.SortOrder
            }).ToList(),
            Products = products.Select(MapToProductResponse).ToList(),
            Tags = node.Tags.Select(t => new TagResponse { Id = t.Id, Name = t.Name }).ToList(),
            Inclusions = node.Inclusions.Select(i => new InclusionResponse 
            { 
                Id = i.Id, 
                Name = i.Name, 
                Description = i.Description, 
                Type = i.Type 
            }).ToList(),
            GuideLanguages = node.GuideLanguages.Select(l => new GuideLanguageResponse 
            { 
                LanguageId = l.LanguageId, 
                Name = l.Name, 
                GuideType = l.GuideType 
            }).ToList(),
            Itinerary = itinerary
        };
    }

    private static ProductResponse MapToProductResponse(ProductNode p) => new()
    {
        Id = p.Id,
        Slug = p.Slug,
        Title = p.Title,
        Description = p.Description,
        DurationMinutes = p.DurationMinutes,
        DurationDescription = p.DurationDescription,
        CancelPolicyHours = p.CancelPolicyHours,
        CancelPolicyText = p.CancelPolicyText,
        MaxGroupSize = p.MaxGroupSize,
        MinParticipants = p.MinParticipants,
        IsPrivate = p.IsPrivate,
        PriceTiers = p.PriceTiers.Select(t => new PriceTierResponse
        {
            Id = t.Id,
            TicketCategoryId = t.TicketCategoryId,
            CategoryName = t.CategoryName,
            Price = t.Price,
            CurrencyCode = t.CurrencyCode
        }).ToList()
    };
}

