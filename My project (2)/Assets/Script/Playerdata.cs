using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class PlayerData
{
    public string Email;
    public string Username;
    public string Password;
    public string ID;
    public string Name; 
    public int Age;
    public int Time;
    public int Score=0;
    public int Highscore=0;
    public string image;
    public PlayerData(string Username ,string Password, string Email,string ID)
    {
        this.Email = Email;
        this.Username = Username;
        this.Password = Password;     
        this.ID = ID;
    }
    public PlayerData(string Email, string Username, string Password, string ID, string Name,int Age,int Time,int Score,int Highscore,string image)
    {
        this.Email = Email;
        this.Username=Username;
        this.Password=Password;     
        this.ID = ID;
        this.Age=Age;  
        this.Time=Time;
        this.Score=Score;
        this.Highscore=Highscore;
        this.image=image;               
    }




}

