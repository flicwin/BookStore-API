using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Authors in the book store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>The full list of all authors from the database</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attempted to GET all Authors.");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("Successfully GOT all Authors.");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Get authors by Id
        /// </summary>
        /// <returns>The author or authors matching "Id" value from the database</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                _logger.LogInfo($"Attempted to GET author with Id:{id}.");
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn($"Author with Id:{id} was not found.");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo($"Successfully GOT the Author with Id:{id}.");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Creates a new Author record
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                _logger.LogInfo($"An Author submission was attempted.");
                if (authorDTO == null)
                {
                    _logger.LogWarn($"An empty Author submission was made.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"An incomplete Author request was submitted.");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Create(author);
                if (!isSuccess)
                {
                    return internalError($"The Create Author request failed. Check the data being submitted.");
                }
                _logger.LogInfo("New Author record created successfully.");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return internalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Updates an Author Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            try
            {
                _logger.LogInfo($"Update Author record attempted with Id:{id}.");

                if (id < 1 || authorDTO == null || id != authorDTO.Id)
                {
                    _logger.LogWarn($"An empty Author Update submission was made with Id:{id}.");
                    return BadRequest();
                }
                var isExists = await _authorRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"(Update) Author with Id:{id} failed. Not found.");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"An incomplete Author Update request was submitted with Id:{id}.");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var success = await _authorRepository.Update(author);
                if (!success)
                {
                    return internalError($"The Update Author operation with Id:{id} failed.");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return internalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Removes the Author record permanently
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"Delete Author record attempted with Id:{id}.");
                if (id < 1)
                {
                _logger.LogWarn($"Delete Author record failed with Id:{id}. Bad data.");
                    return BadRequest();
                }
                var isExists = await _authorRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"(Update) Author with Id:{id} failed. Not found.");
                    return NotFound();
                }
                var author = await _authorRepository.FindById(id);
                var isSuccess = await _authorRepository.Delete(author);
                if (!isSuccess)
                {
                    return internalError($"The Delete Author operation failed.");
                }
                _logger.LogInfo($"Author with Id:{id} deleted successfully.");
                return NoContent();
            }
            catch (Exception e)
            {
                return internalError($"{e.Message} - {e.InnerException}");
            }
        }
        private ObjectResult internalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact Flic.");
        }
    }
}
