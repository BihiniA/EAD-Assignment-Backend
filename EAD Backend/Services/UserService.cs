using MongoDBExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDBExample.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using EAD_Backend.JWTAuthentication;
using EAD_Backend.Dto;

public class UserService
{
    private readonly IMongoCollection<Users> _UserCollection;
    //private readonly JwtAuthenticationService _jwtAuthenticationService;

    //public UserService(JwtAuthenticationService jwtAuthenticationService)
    //{
    //    _jwtAuthenticationService = jwtAuthenticationService;
    //}


    public UserService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _UserCollection = database.GetCollection<Users>("users");
    }



    public async Task<Users> CreateAsync(CreateUserDto user) //create user
    {
        try
        {
            var pass = EncodePasswordToBase64(user.password);

            Users userObj = new Users
            {
                email = user.email,
                password = pass,
                name = user.name,
                nic = user.nic,
                Role = UserRole.User,
                Status = EAD_Backend.Models.StatusEnum.ACTIVE
            };

            await _UserCollection.InsertOneAsync(userObj);
            return userObj;
        }
        catch (System.Exception)
        {
            throw;
        }

    }

    public async Task<List<Users>> GetAsync() //get all users
    {

        try
        {
            return await _UserCollection.Find(new BsonDocument()).ToListAsync();
        }
        catch (System.Exception)
        {
            throw;
        }

    }

    public async Task AddToUsers(string id, string password) // change user password using docuemnt id
    {

        try
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_id", id);
            UpdateDefinition<Users> update = Builders<Users>.Update.Set("password", password);
            await _UserCollection.UpdateOneAsync(filter, update);
            return;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(string id) //delete user
    {
        try
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_id", id);
            await _UserCollection.DeleteOneAsync(filter);
            return;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task UpdateUser(string id, Users user) // update user
    {
        try
        {
            await _UserCollection.ReplaceOneAsync(x => x.nic == id, user);
            return;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    // public async Task<bool> UpdateUserStatus(string nic, string newStatus) // update user status only
    //{
    // var filter = Builders<Users>.Filter.Eq(u => u.nic, nic);
    // var update = Builders<Users>.Update.Set(u => u.Status, newStatus);

    // var result = await _UserCollection.UpdateOneAsync(filter, update);

    // return result.ModifiedCount > 0;
    //}

    //user login check

    public async Task<string> Login(string email, string password)
    {
        var user = await GetUserByEmail(email);
        var userPassword = EncodePasswordToBase64(password);
        var pass = DecodeFrom64(user.password);

        if (user == null || userPassword != pass)
        {
            throw new Exception("Invalid username or password");
        }

        return GenerateJSONWebToken(user);
    }

    public string GenerateJSONWebToken(Users users) // token generation 
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzINi"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
       {
                new Claim("email",users.email), //add email to token
                new Claim("password",users.password) // add password to token
        };
        var token = new JwtSecurityToken("EADBackend",
          "EADBackend",
          claims,
          expires: DateTime.Now.AddMinutes(120), // token expiery time
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Users> GetUser(string id)//find single user
    {
        try
        {
            var user = await _UserCollection.Find(x => x.nic
            == id).FirstOrDefaultAsync();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<Users> GetUserByEmail(string email) // get user by email
    {
        try
        {
            var user = await _UserCollection.Find(x => x.email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public static string EncodePasswordToBase64(string password)
    {
        try
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    }
    //this function Convert to Decord your Password
    public string DecodeFrom64(string encodedData)
    {
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        System.Text.Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encodedData);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        string result = new String(decoded_char);
        return result;
    }

    internal Task<bool> UpdateUserStatus(string id, object status)
    {
        throw new NotImplementedException();
    }

    public async Task<UserRegisterDto> Register(UserRegisterDto userRegisterDto)
    {
        var user = await GetUserByEmail(userRegisterDto.email);

        if(user != null)
        {
            throw new Exception("User already registered");
        }

        var password = EncodePasswordToBase64(userRegisterDto.password);

        Users userObj = new Users { 
            email = userRegisterDto.email,
            password = password,
            name = userRegisterDto.name,
            nic = userRegisterDto.nic,
            Role = userRegisterDto.Role,
            Status = EAD_Backend.Models.StatusEnum.ACTIVE
        };

        await _UserCollection.InsertOneAsync(userObj);
        return userRegisterDto;

    }
}