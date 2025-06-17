using EngAI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EngAI.Controllers;


[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    // POST: CourseController/Create
    [HttpPost]
    public ActionResult Create(Course course)
    {
        try
        {
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public ActionResult<Course?> Get(int id)
    {
        try
        {
            return Ok(null);
        }
        catch
        {
            return BadRequest();
        }
    }

    // GET: CourseController/Edit/5
    [HttpPost]
    public ActionResult Edit(int id)
    {
        try
        {
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    // GET: CourseController/Delete/5
    [HttpPost]
    public ActionResult Delete(int id)
    {
        try
        {
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}
