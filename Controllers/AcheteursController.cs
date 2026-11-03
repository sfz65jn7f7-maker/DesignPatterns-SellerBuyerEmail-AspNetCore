using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InscriptionApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AcheteursController : ControllerBase
{
    private readonly IAcheteurService _service;
    private readonly IInscriptionFacade _facade;

    public AcheteursController(IAcheteurService service, IInscriptionFacade facade)
    {
        _service = service;
        _facade = facade;
    }

    [HttpPost]
    public async Task<ActionResult<AcheteurResponse>> Inscrire(AcheteurCreateRequest request)
    {
        var created = await _facade.InscrireAcheteurAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AcheteurResponse>> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AcheteurResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost("{id:int}/confirmer")]
    public async Task<ActionResult<AcheteurResponse>> Confirmer(int id)
        => Ok(await _service.ChangerStatutAsync(id, ActionStatut.Confirmer));

    [HttpPost("{id:int}/suspendre")]
    public async Task<ActionResult<AcheteurResponse>> Suspendre(int id)
        => Ok(await _service.ChangerStatutAsync(id, ActionStatut.Suspendre));

    [HttpPost("{id:int}/reactiver")]
    public async Task<ActionResult<AcheteurResponse>> Reactiver(int id)
        => Ok(await _service.ChangerStatutAsync(id, ActionStatut.Reactiver));
}