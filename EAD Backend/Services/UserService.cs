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

public class UserService
{
    private readonly IMongoCollection<Users> _UserCollection;


    public UserService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _UserCollection = database.GetCollection<Users>("users");
    }



    public async Task<Users> CreateAsync(Users user) //create user
    {
     
        try
        {
            await _UserCollection.InsertOneAsync(user);
            return user;
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

    public async Task<Users> Login(string email, string password) //login validation using email and password
    {
        try
        {
            var user = await _UserCollection.Find(x => x.email == email && x.password == password).FirstOrDefaultAsync();
            if (user != null)
            {
                return user;
            }
            else
            {
                return user;
            }
        }
        catch (System.Exception)
        {
            throw;
        }
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

    internal Task<bool> UpdateUserStatus(string id, object status)
    {
        throw new NotImplementedException();
    }
}