using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public string Email;
    public string Username;
    public string Password;
    public string ID;
    public string Name;
    public int Age;
    public int score = 0;
    public int Highscore = 0;
    public string image;
    public PlayerData(string email, string userName, string password, string name, int age, string iD, int score, int highscore, string image)
    {
        this.Email = email;
        this.Username = userName;
        this.Password = password;
        this.ID = iD;
        this.Name = name;
        this.Age = age;      
        this.score = score;
        this.Highscore = highscore;
        this.image = image;
    }
    public PlayerData(string Email, string userName, string password, string Id) {
        this.Email = Email;
        this.Username=userName;
        this.Password=password; 
        this.ID=Id;
    }
}
