using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;


public class FormController : MonoBehaviour
{
    public GameObject loginform, signupform, profileform, forgetPassform, notification, MainMenu,changepassform,changemailform;
    [SerializeField] 
    public TMP_InputField loginUser, loginPassword, signupEmail, signupUser, signupPassword, signupConfirm;
    public TMP_InputField Age_txt, Name_txt,NewPassword, PsCheckEmail,CheckEmail ,ConfirmPass, ConfirmEmail, NewEmail;
    public TMP_InputField RcvEmail,RcvUser,RcvID ;
    public Button submitLogin, submitSignup;
    public TMP_Text noti_message, email_used, user_used,Email_txt,Username_txt,ID_text;
    public TMP_Text[] messages;
    //tao list chua thong tin de khi chay truong se doc ko can dung append
    List<PlayerData> playerlist = new List<PlayerData>();
    
    public void RunLogin()
    {
        loginform.SetActive(true);
        signupform.SetActive(false);
        profileform.SetActive(false);
        forgetPassform.SetActive(false);
        MainMenu.SetActive(false);
        notification.SetActive(false);
    }
    public void Runsignup()
    {
        loginform.SetActive(false);
        signupform.SetActive(true);
        profileform.SetActive(false);
        forgetPassform.SetActive(false);
    }
    public void Runprofile()
    {
        loginform.SetActive(false);
        signupform.SetActive(false);
        profileform.SetActive(true);
        forgetPassform.SetActive(false);  
        changemailform.SetActive(false);
        changepassform.SetActive(false);
    }
    public void RunForgetPass()
    {
        loginform.SetActive(false);
        signupform.SetActive(false);
        profileform.SetActive(false);
        forgetPassform.SetActive(true);
    }
    public void LoginUser()
    {
        //Kiem tra truong ten va mat khau co trong ko
        if (string.IsNullOrEmpty(loginUser.text) || string.IsNullOrEmpty(loginPassword.text)) {
            showNotification("Fields not empty", false);
            return;
        }
        //Dang nhap
        List<PlayerData> playerexisted = Function.Readinfo<PlayerData>();
        PlayerData playerdata = playerexisted.Find(player => player.Username == loginUser.text && player.Password == loginPassword.text);
        if (playerdata != null)
        {
            Debug.Log("Login successfully");
            loginPassword.text = "";
            //Load data cua ng choi khi dang nhap thanh cong
            Name_txt.text = playerdata.Name;
            Age_txt.text = playerdata.Age.ToString();
            Email_txt.text = playerdata.Email;
            Username_txt.text = playerdata.Username;
            ID_text.text = playerdata.ID;
            //mo menu
            loginform.SetActive(false);
            MainMenu.SetActive(true);
        }
        else
        {
            showNotification("Username or password not correct",false);
            loginPassword.text = "";
            return;
        }

    }
    public void UpdateInfo(Button button)
    {
        PlayerData playerdata = playerlist.Find(player => player.Username == Username_txt.text);
        
        if (button == buttons[3])
        {
            playerdata.Name = Name_txt.text;
            playerdata.Age = int.Parse(Age_txt.text);           
            StartCoroutine(ShowMessage("Changes saved",0,2));
        }
        if (button == buttons[4])
        {
            if (string.IsNullOrEmpty(NewPassword.text)|| string.IsNullOrEmpty(ConfirmPass.text)||string.IsNullOrEmpty(PsCheckEmail.text))
            {
                showNotification("Fields not empty", false);
                return;
            }
            if (PsCheckEmail.text != playerdata.Email)
            {
                messages[1].text="Email not correct";
                messages[2].text = "";
                NewPassword.text = "";
                ConfirmPass.text = "";
                return;
            }
            else
            {
                messages[1].text = "";
                if (NewPassword.text == ConfirmPass.text) playerdata.Password = NewPassword.text;
                else {
                    messages[2].text = "Password confirmation does not match";
                    ConfirmPass.text = "";
                    return;
                }
            }
            showNotification("Password change successfully", false);
            
            Runprofile();
        }
        if (button == buttons[5])
        {
            if (string.IsNullOrEmpty(NewEmail.text) || string.IsNullOrEmpty(ConfirmEmail.text) || string.IsNullOrEmpty(CheckEmail.text))
            {
                showNotification("Fields not empty", false);
                return;
            }
            if (CheckEmail.text != playerdata.Email)
            {
                messages[3].text = "Email not correct";
                messages[4].text = "";
                NewEmail.text = "";
                ConfirmEmail.text = "";
                return;
            }
            else
            {
                messages[3].text = "";
                if (NewEmail.text == ConfirmEmail.text) playerdata.Email = NewEmail.text;
                else
                {
                    messages[4].text = "Email confirmation does not match";
                    ConfirmEmail.text = "";
                    return;
                }
            }
            showNotification("Email change successfully", false);
            Runprofile();
        }
        if (button == buttons[6])
        {
            if (string.IsNullOrEmpty(RcvEmail.text) || string.IsNullOrEmpty(RcvUser.text) || string.IsNullOrEmpty(RcvID.text))
            {
                showNotification("Fields not empty", false);
                return;
            }
            playerdata = playerlist.Find(player => player.Email == RcvEmail.text  && player.Username == RcvUser.text  &&  player.ID== RcvID.text);
            if (playerdata!=null)
            {
                string colortext = string.Format("<color=#ff0000>{0}</color>", playerdata.Password);
                showNotification("Your password is \n\"{0}\"" +colortext, false);
                RcvUser.text = "";
                RcvEmail.text = "";
                RcvID.text = "";
            }
            else
            {
                showNotification("Information incorrect", false);
                return;
            }                    
        }
           
        Function.Saveinfo<PlayerData>(playerlist);
            
    }
    public void SignUpUser()
    {
        
        //submitSignup.interactable = (signupUser.text.Length >= 6 && signupPassword.text.Length >= 6);
        if (string.IsNullOrEmpty(signupUser.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupConfirm.text)||string.IsNullOrEmpty(signupEmail.text))
        {
            showNotification("Fields not empty",false);
            return;
        }
        List<PlayerData> playerexisted = Function.Readinfo<PlayerData>();
        bool emailisused = playerexisted.Any(existed => existed.Email == signupEmail.text);
        bool userisused = playerexisted.Any(existed => existed.Username == signupUser.text);      
        if (userisused||emailisused)
        {   

            if (userisused)
            {
                user_used.text = "Username have already been used";
            }
            if (emailisused)
            {
                email_used.text = "Email have already been used";
            }
            if (!emailisused)
            {
                email_used.text = "";
            }
            if (!userisused)
            {
                user_used.text = "";
            }
            signupPassword.text = "";
            signupConfirm.text = "";
            Debug.Log("existed");
            return;
        }
        playerlist.Add(new PlayerData(signupUser.text, signupPassword.text, signupEmail.text, "PL" + Guid.NewGuid().ToString().Substring(0, 4)));
        email_used.text = "";
        user_used.text = "";
        signupPassword.text = "";
        signupConfirm.text = "";
        Function.Saveinfo<PlayerData>(playerlist);
        showNotification("Account created successfully",false);
        RunLogin();
        //Dang ki
    }
    private void showNotification(string message,bool yesno)
    {
        if (yesno)
        {
            NotiBtnSelect(buttons[2],true);
            
        }
        else {
            NotiBtnSelect(buttons[2], false);
            
        }
        
        notification.SetActive(true);
        noti_message.text = " " + message;
        
    }
    IEnumerator ShowMessage(string message,int i,float delay)
    {
        messages[i].text=message;
        yield return new WaitForSeconds(delay);
        messages[i].text = "";
    }
    public void Noti_button()
    {
        notification.SetActive(false);   
    }
    public Button[] buttons;

    public void NotiBtnSelect(Button button,bool yesno)
    {
        for (int i = 0; i <= 2; i++)
        {
            if (yesno)
            {
                if (button == buttons[i])
                {
                    buttons[i].interactable = false;
                    buttons[i].gameObject.SetActive(false);
                }
                else
                {
                    buttons[i].interactable = true;
                    buttons[i].gameObject.SetActive(true);
                }

            }
            else
            {              
                    if (button == buttons[i])
                    {
                        buttons[i].interactable = true;
                        buttons[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        buttons[i].interactable = false;
                        buttons[i].gameObject.SetActive(false);
                    }               
            }
        }

    }

    public void Exit()
    {       
        showNotification("Exit game?",true);
        Debug.Log("exit");
        buttons[1].onClick = new Button.ButtonClickedEvent();
        buttons[1].onClick.AddListener(Application.Quit);
    }
    public void Logout()
    {         
        showNotification("Are you sure you want to log out ",true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    private void Start()
    {
        playerlist = Function.Readinfo<PlayerData>();
        RunLogin();
    }
}
