using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Serpent5.Xrefs.Host.Controllers;

[ApiController]
[Route("/api/Suggestions")]
public class SuggestionsController : ControllerBase
{
    private readonly XrefClient xrefClient;

    public SuggestionsController(XrefClient xrefClient)
        => this.xrefClient = xrefClient;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get(
        [FromQuery(Name = "q"), Required] string queryText, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<XrefSuggestion> xrefSuggestions;

        try
        {
            xrefSuggestions = await xrefClient.SuggestAsync(queryText, cancellationToken);
        }
        catch (XrefClientException)
        {
            // TODO: Handle Errors.
            xrefSuggestions = Array.Empty<XrefSuggestion>();
        }

        return new(xrefSuggestions.Select(x => x.Uid));
    }
}
