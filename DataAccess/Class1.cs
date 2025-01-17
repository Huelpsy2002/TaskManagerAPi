using Data;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
namespace DataAccessLayer
{
    public static class DataAccess
    {
        private static readonly string connectionString = Environment.GetEnvironmentVariable("connection_string");


        public static async Task<List<getTasksDto>> getAllTasks(int userId)
        {





            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("getAllTasks", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@userId", userId);

            List<Data.getTasksDto> Tasks = new List<Data.getTasksDto>();

            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                  
                    while (await reader.ReadAsync())
                    {
                        
                        Tasks.Add(
                            new Data.getTasksDto(
                                
                                taskId:(int)reader["TaskId"],
                                title : (string)reader["title"],
                                description: (string)reader["Description"],
                                status: (string)reader["Status"],
                                catgeory: (string)reader["CategoryName"],
                                createdAt: (DateTime)reader["createdAt"],
                                updatedAt: (DateTime)reader["updatedAt"]

                                


                                )
                            );

                    }
                }


                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return Tasks;
        }




        public static async Task<getTasksDto>getTask(int userId,int taskId)
        {





            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("getTask", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@taskId", taskId);

            getTasksDto task = null;
            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {

                    while (await reader.ReadAsync())
                    {


                        task = new Data.getTasksDto(

                              taskId: (int)reader["TaskId"],
                              title: (string)reader["title"],
                              description: (string)reader["Description"],
                              status: (string)reader["Status"],
                              catgeory: (string)reader["CategoryName"],
                              createdAt: (DateTime)reader["createdAt"],
                              updatedAt: (DateTime)reader["updatedAt"]




                              );
                            

                    }
                }


                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return task;
        }

        public static async Task<getTasksDto?> addTask(int userId,addTaskdDto addTaskdDto)
        {


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("AddTask", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@title", addTaskdDto.title);
            command.Parameters.AddWithValue("@descreption", addTaskdDto.description);
            command.Parameters.AddWithValue("@categoryId", addTaskdDto.catgeoryId);
            command.Parameters.AddWithValue("@UserId", userId);

            var taskIdParam = new SqlParameter("@TaskId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(taskIdParam);

            bool excuted = false;
            getTasksDto task = null;
            try
            {
                await connection.OpenAsync();
                int rowEfectted = await command.ExecuteNonQueryAsync();
                if (rowEfectted > 0)
                {
                    task = await getTask(userId, (int)taskIdParam.Value);
                    excuted = true;

                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return excuted == true ? task : null;

        }



        public static async Task<getTasksDto?> updateTask(int userId,updateTaskDto updatetaskdto)
        {


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("updateTask", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@title", updatetaskdto.title);
            command.Parameters.AddWithValue("@taskId", updatetaskdto.taskId);
            command.Parameters.AddWithValue("@descreption", updatetaskdto.description);
            command.Parameters.AddWithValue("@categoryId", updatetaskdto.catgeoryId);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@status", updatetaskdto.status);

            

            bool excuted = false;
            getTasksDto task = null;
            try
            {
                await connection.OpenAsync();
                int rowEfectted = await command.ExecuteNonQueryAsync();
                if (rowEfectted > 0)
                {
                    task = await getTask(userId, updatetaskdto.taskId);
                    excuted = true;

                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return excuted == true ? task : null;

        }


        public static async Task<bool> deleteTask(int userId,int taskId)
        {


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("deleteTask", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@taskId", taskId);
            command.Parameters.AddWithValue("@userId", userId);
            bool excuted = false;

            try
            {
                await connection.OpenAsync();
                int rowEfectted = await command.ExecuteNonQueryAsync();
                if (rowEfectted > 0)
                {
                    excuted = true;

                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return excuted;

        }



        //public static async Task<getUserDto?> getUser(string username)
        //{

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    SqlCommand command = new SqlCommand("getUser", connection);
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.AddWithValue("@username", username);
        //    getUserDto user = null;
        //    try
        //    {
        //        await connection.OpenAsync();
        //        SqlDataReader reader = await command.ExecuteReaderAsync();

        //        if (reader.HasRows)
        //        {
        //            while(await reader.ReadAsync()){
        //                user = new getUserDto(
        //                userId: (int)reader["UserId"],
        //                username: (string)reader["UserName"],
        //                firstname: (string)reader["FirstName"],
        //                lastname: (string)reader["LastName"],
        //                password: (string)reader["PassWord"]





        //                );
        //            }
                    
                    

        //        }




        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return user;
        //}
        public static async Task<loginUserDto?> LogedUser(string username)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("getUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            loginUserDto user = null;
            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        user = new loginUserDto
                        (
                            username : (string)reader["UserName"],
                            password : (string)reader["PassWord"]


                        );
                    }



                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return user;
        }


        public static async Task<getUserDto?> getUser(string username)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("getUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@username", username);
            getUserDto user = null;
            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {

                        user = new getUserDto(

                            userId: (int)reader["UserId"],
                            username: (string)reader["UserName"],
                            firstname: (string)reader["FirstName"],
                            lastname: (string)reader["LastName"],
                            createdat: (DateTime)reader["createdAt"],
                            updatedat: (DateTime)reader["updatedAt"]



                            );
                    }



                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return user;
        }

        public static async Task<getUserDto?> addUser(addUserDto adduserdto)
        {


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("AddUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", adduserdto.username);
            command.Parameters.AddWithValue("@Password", adduserdto.password);
            command.Parameters.AddWithValue("@FirstName", adduserdto.firstName);
            command.Parameters.AddWithValue("@LastName", adduserdto.lastName);
            var userIdParam = new SqlParameter("@userId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var createdAtParam = new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
            command.Parameters.Add(userIdParam);
            command.Parameters.Add(createdAtParam);




            bool excuted = false;
            getUserDto user = null;
            try
            {
                await connection.OpenAsync();
                int rowEfectted = await command.ExecuteNonQueryAsync();
                if (rowEfectted > 0)
                {

                    user = new getUserDto(
                        userId: (int)userIdParam.Value,
                        username: adduserdto.username,
                        firstname: adduserdto.firstName,
                        lastname: adduserdto.lastName,
                        createdat: (DateTime)createdAtParam.Value,
                        updatedat: (DateTime)createdAtParam.Value);


                    
                    excuted = true;

                }




            }
            catch (SqlException ex) when (ex.Number == 2627)
            {

                Console.WriteLine("Error");
                throw new Exception("The username you entered already exists. Please use a different one.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return excuted == true ? user : null;

        }

        public static async Task<getUserDto?> updateUser(updateUserDto updateuserdto)
        {


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("updateUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", updateuserdto.userId);
            command.Parameters.AddWithValue("@UserName", updateuserdto.username);
            command.Parameters.AddWithValue("@Password", updateuserdto.password);
            command.Parameters.AddWithValue("@FirstName", updateuserdto.firstName);
            command.Parameters.AddWithValue("@LastName", updateuserdto.lastName);
            command.Parameters.AddWithValue("@isActive", updateuserdto.isActive);

            var updatedAtParam = new SqlParameter("@updatedAt", SqlDbType.DateTime) { Direction = ParameterDirection.Output };
            command.Parameters.Add(updatedAtParam);

            bool excuted = false;
            getUserDto user = null;
            try
            {
                await connection.OpenAsync();
                int rowEfectted = await command.ExecuteNonQueryAsync();
                if (rowEfectted > 0)
                {
                    user = await getUser(updateuserdto.username);
                    user.updatedAt = (DateTime)updatedAtParam.Value;
                    excuted = true;

                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return excuted == true ? user : null;

        }


        public static async Task<bool> deleteUser(int userId)
        {


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("DeleteUser", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", userId);
            bool excuted = false;

            try
            {
                await connection.OpenAsync();
                int rowEfectted = await command.ExecuteNonQueryAsync();
                if (rowEfectted > 0)
                {
                    excuted = true;

                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return excuted;

        }
    }





}
