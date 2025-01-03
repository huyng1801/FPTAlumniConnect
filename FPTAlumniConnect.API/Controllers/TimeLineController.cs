using AutoMapper;
using FPTAlumniConnect.API.Services.Interfaces;
using FPTAlumniConnect.BusinessTier.Constants;
using FPTAlumniConnect.BusinessTier.Payload;
using FPTAlumniConnect.BusinessTier.Payload.EventTimeLine;
using FPTAlumniConnect.DataTier.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTAlumniConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeLineController : BaseController<TimeLineController>
    {
        private readonly ITimeLineService _timeLineService;
        private readonly IMapper _mapper;

        public TimeLineController(
            ILogger<TimeLineController> logger,
            ITimeLineService timeLineService,
            IMapper mapper) : base(logger)
        {
            _timeLineService = timeLineService;
            _mapper = mapper;
        }

        [HttpPost(ApiEndPointConstant.TimeLine.TimeLinesEndPoint)]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTimeLine([FromBody] TimeLineInfo request)
        {
            try
            {
                int timeLineId = await _timeLineService.CreateTimeLine(request);
                return CreatedAtAction(nameof(GetTimeLineById), new { id = timeLineId }, timeLineId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating timeline.");
                return BadRequest("An error occurred while creating the timeline.");
            }
        }

        [HttpGet(ApiEndPointConstant.TimeLine.TimeLineEndPoint)]
        [ProducesResponseType(typeof(TimeLineReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTimeLineById(int id)
        {
            try
            {
                TimeLineReponse timeLine = await _timeLineService.GetTimeLineById(id);
                return Ok(timeLine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Timeline not found.");
                return NotFound("Timeline not found.");
            }
        }

        [HttpPut(ApiEndPointConstant.TimeLine.TimeLineEndPoint)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTimeLine(int id, [FromBody] TimeLineInfo request)
        {
            try
            {
                bool isUpdated = await _timeLineService.UpdateTimeLine(id, request);
                if (isUpdated)
                {
                    return Ok("Timeline updated successfully.");
                }
                else
                {
                    return NotFound("Timeline not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating timeline.");
                return BadRequest("An error occurred while updating the timeline.");
            }
        }

        [HttpGet(ApiEndPointConstant.TimeLine.TimeLinesEndPoint)]
        [ProducesResponseType(typeof(IPaginate<TimeLineReponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewAllTimeLines([FromQuery] TimeLineFilter filter, [FromQuery] PagingModel pagingModel)
        {
            try
            {
                IPaginate<TimeLineReponse> timeLines = await _timeLineService.ViewAllTimeLine(filter, pagingModel);
                return Ok(timeLines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching timelines.");
                return BadRequest("An error occurred while fetching the timelines.");
            }
        }
    }
}
