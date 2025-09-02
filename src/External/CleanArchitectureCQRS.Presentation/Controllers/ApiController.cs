using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureCQRS.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    public readonly IMediator _mediator;

    public ApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [NonAction]
    public IActionResult ResultedAction<T>(Result<T> result)
    {
        if (result == null)
        {
            // Result nesnesi null ise, bu bir yanlış kullanım veya beklenmeyen durumdur.
            // Bu durumda 400 BadRequest ile hata bildiriyoruz.
            return BadRequest("Result cannot be null.");
        }

        // ResultStatus durumuna göre HTTP yanıtı oluşturuluyor.
        // switch ifadesiyle okunabilirlik artırılıyor.
        return result.Status switch
        {
            ResultStatus.Success => Ok(result), // Başarı: 200 OK + data
            ResultStatus.Error => BadRequest(result), // Hata: 400 BadRequest + hata detayları
            ResultStatus.NotFound => NotFound(result), // Bulunamadı: 404 NotFound + hata mesajları
            ResultStatus.ValidationError => BadRequest(result), // Doğrulama hatası: 400 BadRequest
            ResultStatus.NoContent => NoContent(), // İçerik yok: 204 No Content

            ResultStatus.UnAuthorized => Unauthorized(), // Yetkisiz: 401 Unauthorized + hata
            
            _ => StatusCode(500, result) // Diğer tüm durumlar için: 500 Internal Server Error
        };
    }
    [NonAction]
    public IActionResult ResultedAction(Result result)
    {
        if (result == null)
        {
            // Result nesnesi null ise, bu bir yanlış kullanım veya beklenmeyen durumdur.
            // Bu durumda 400 BadRequest ile hata bildiriyoruz.
            return BadRequest("Result cannot be null.");
        }

        // ResultStatus durumuna göre HTTP yanıtı oluşturuluyor.
        return result.Status switch
        {
            ResultStatus.Success => Ok(result), // Başarı: 200 OK + data
            ResultStatus.Error => BadRequest(result), // Hata: 400 BadRequest + hata detayları
            ResultStatus.NotFound => NotFound(result), // Bulunamadı: 404 NotFound + hata mesajları
            ResultStatus.ValidationError => BadRequest(result), // Doğrulama hatası: 400 BadRequest
            ResultStatus.NoContent => NoContent(), // İçerik yok: 204 No Content

            ResultStatus.UnAuthorized => Unauthorized(), // Yetkisiz: 401 Unauthorized + hata
            
            _ => StatusCode(500, result) // Diğer tüm durumlar için: 500 Internal Server Error
        };
    }
}