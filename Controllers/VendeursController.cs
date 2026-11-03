using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InscriptionApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendeursController : ControllerBase
{
    private readonly IVendeurService _service;
    private readonly IInscriptionFacade _facade;

    public VendeursController(IVendeurService service, IInscriptionFacade facade)
    {
        _service = service;
        _facade = facade;
    }

    [HttpPost]
    public async Task<ActionResult<VendeurResponse>> Inscrire(VendeurCreateRequest request)
    {
        var created = await _facade.InscrireVendeurAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VendeurResponse>> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VendeurResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost("{id:int}/confirmer")]
    public async Task<ActionResult<VendeurResponse>> Confirmer(int id)
        => Ok(await _service.ChangerStatutAsync(id, ActionStatut.Confirmer));

    [HttpPost("{id:int}/suspendre")]
    public async Task<ActionResult<VendeurResponse>> Suspendre(int id)
        => Ok(await _service.ChangerStatutAsync(id, ActionStatut.Suspendre));

    [HttpPost("{id:int}/reactiver")]
    public async Task<ActionResult<VendeurResponse>> Reactiver(int id)
        => Ok(await _service.ChangerStatutAsync(id, ActionStatut.Reactiver));
}