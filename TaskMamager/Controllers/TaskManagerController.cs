using Data;
using BussinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace TaskMamager.Controllers
{
    [Route("api/TaskManager")]
    [ApiController]

    


    public class TaskManagerController : ControllerBase
    {


        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult redirct()
        {
            return Ok(new { message = "task manager api" });
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<getUserDto>> addUser(addUserDto adduserdto)
        {
            try
            {
                var user = await Users.addUser(adduserdto);

                return Ok(user);



            }
            catch (InvalidDataException err)
            {

                return BadRequest(new { error = "invalid data:", message = err.Message });
            }

            
            catch (Exception err)
            {
                if(err.Message == "The username you entered already exists. Please use a different one.")
                {
                    return BadRequest(new { error = "invalid data:", message = "The username you entered already exists. Please use a different one." });

                }
                throw new Exception($"{err.Message}");
            }
        }



        [Authorize]
        [HttpPut("updateuser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<getUserDto>> updateUser(updateUserDto updateuserdto)
        {
           
            try
            {
                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;// get the username of the logged user
                getUserDto user = await Users.getUser(username);
                if (user != null)
                {
                    if (user.userId == updateuserdto.userId) // enusre the logged user update only his data
                    {
                        var updateduser = await Users.updateUser(updateuserdto);

                        return Ok(updateduser);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(new { message = "user is null" });


            }
            catch (InvalidDataException err)
            {

                return BadRequest(new { error = "invalid data:", message = err.Message });
            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.StackTrace}");
            }
        }


        [Authorize]

        [HttpDelete("deleteuser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult> deleteuser(deleteUserDto deleteuserdto)
        {
           
            try
            {

                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;// get the username of the logged user
                getUserDto user = await Users.getUser(username);
                if (user != null)
                {
                    if (user.userId == deleteuserdto.userId) 
                    {
                        if (await Users.deleteUser(deleteuserdto))
                        {
                            return Ok(new { message = "user deleted successfully" });
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(new { message = "user is null" });


               

                




            }
            catch (InvalidDataException err)
            {

                return BadRequest(new { error = "invalid data:", message = err.Message });
            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.Message}");
            }
        }
        [Authorize]

        [HttpGet("{userId}/tasks")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]


        public async Task<ActionResult<getTasksDto>> getAllTasks(int userId)
        {
           




            try
            {
                
                
                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;// get the username of the logged user
                foreach(var u in User.Claims)
                {
                    Console.WriteLine(u);
                }
                getUserDto user = await Users.getUser(username);
                if (user != null)
                {
                    Console.WriteLine(user.userId);
                    if (user.userId == userId)
                    {
                        List<getTasksDto> tasks = await Tasks.getAllTasks(userId);
                        return Ok(tasks);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(new { message = "user is null" });




                

            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.Message}");
            }
        }

        [Authorize]

        [HttpPost("{userId}/tasks/addtask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<addTaskdDto>>addTask(int userId, addTaskdDto addtaskdto)
        {

            
            try
            {

                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;// get the username of the logged user
                getUserDto user = await Users.getUser(username);
                if (user != null)
                {
                    if (user.userId == addtaskdto.userId && user.userId==userId)
                    {
                        var task = await Tasks.addTask(addtaskdto);

                        return Ok(task);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(new { message = "user is null" });

                
                
               
            }
            catch(InvalidDataException err)
            {
                return BadRequest(new { error = "invalid data", message = err.Message });
            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.Message}");
            }

        }

        [Authorize]

        [HttpPut("{userId}/tasks/updatetask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<updateTaskDto>> updateTask(int userId,updateTaskDto updatetaskdto)
        {

            
            try
            {
                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;// get the username of the logged user
                getUserDto user = await Users.getUser(username);
                if (user != null)
                {
                    if (user.userId == updatetaskdto.userId && user.userId==userId)
                    {
                        var task = await Tasks.updateTask(updatetaskdto);

                        return Ok(task);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(new { message = "user is null" });


               


            }
            catch (InvalidDataException err)
            {
                return BadRequest(new { error = "invalid data", message = err.Message });
            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.Message}");
            }

        }


        [Authorize]

        [HttpDelete("{userId}/tasks/deletetask")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> deleteTask(int userId,deleteTaskDto deletetaskdto)
        {

           
            try
            {


                var username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;// get the username of the logged user
                getUserDto user = await Users.getUser(username);
                if (user != null)
                {
                    if (user.userId == deletetaskdto.userId && user.userId == userId)
                    {
                        if (await Tasks.deleteTask(deletetaskdto))
                        {
                            return Ok(new { message = "task deleted successfully" });
                        }
                        else
                        {
                            return BadRequest(new { message = "failed deleting task" });
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(new { message = "user is null" });




               


            }
            catch (InvalidDataException err)
            {
                return BadRequest(new { error = "invalid data", message = err.Message });
            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.Message}");
            }

        }




        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<getUserDto>> TaskManagerLogin( loginUserDto userdto)
        {

            try
            {
                string token = "" ;
                var user = await Users.LogUser(userdto.username, userdto.password);
                if (user != null)
                {
                     token = TokenService.GenerateToken(user.username);

                }

                return Ok(new {user, token});



            }
            catch (InvalidDataException err)
            {
                return BadRequest(new { error = "error", message = err.Message });
            }
            catch (Exception err)
            {
                throw new Exception(@$"An unexpected error occurred.
                                     {err.Message}");
            }



        }
    }
}
