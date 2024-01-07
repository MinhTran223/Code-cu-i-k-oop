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
using System.IO;
using UnityEngine.Networking;



public class FormController : MonoBehaviour
{
    public GameObject loginform, signupform, profileform, forgetPassform, notification, MainMenu,changepassform,changemailform;
    [SerializeField] 
    public TMP_InputField loginUser, loginPassword, signupEmail, signupUser, signupPassword, signupConfirm;
    public TMP_InputField Age_txt, Name_txt,NewPassword, PsCheckEmail,CheckEmail ,ConfirmPass, ConfirmEmail, NewEmail;
    public TMP_InputField RcvEmail,RcvUser,RcvID ;
    public Button submitLogin, submitSignup;
    public TMP_Text noti_message,Email_txt,Username_txt,ID_text;
    public TMP_Text[] messages;
    public Button[] buttons;
    public RawImage image;
    //tao list chua thong tin de khi chay truong se doc ko can dung append
    List<PlayerData> playerlist = new List<PlayerData>();
    //Chay form theo ten
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
    //Nhan nut dang nhap
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
            string eml=playerdata.Email.Substring(0,3)+new string ('*',playerdata.Email.Length-3); 
            Email_txt.text = eml;
            Username_txt.text = playerdata.Username;
            ID_text.text = playerdata.ID;
            image =stringtoImage(playerdata.image);
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
    //Cap nhat tt nguoi choi va tt tai khoan
    public void UpdateInfo(Button button)
    {
        PlayerData playerdata = playerlist.Find(player => player.Username == Username_txt.text);
        
        if (button == buttons[3])
        {
            playerdata.Name = Name_txt.text;
            playerdata.Age = int.Parse(Age_txt.text);
            playerdata.image = Imagetostring(image);
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
                showNotification(string.Format("Your password is \n\"<color=#ff0000>{0}</color>\" ", playerdata.Password), false);
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
    //Nhan nut dang ki
    public void SignUpUser()
    {
        messages[5].text = "";
        messages[6].text = "";
        messages[7].text = "";
        if (signupUser.text.Length >= 6 && signupPassword.text.Length >= 6 && signupEmail.text.Length >= 6)
        {
            if (string.IsNullOrEmpty(signupUser.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupConfirm.text) || string.IsNullOrEmpty(signupEmail.text))
            {
                showNotification("Fields not empty", false);
                return;
            }
            List<PlayerData> playerexisted = Function.Readinfo<PlayerData>();
            bool emailisused = playerexisted.Any(existed => existed.Email == signupEmail.text);
            bool userisused = playerexisted.Any(existed => existed.Username == signupUser.text);
            if (userisused || emailisused)
            {

                if (userisused)
                {
                    messages[7].text = "Username have already been used";
                }
                if (emailisused)
                {
                    messages[6].text = "Email have already been used";
                }
               
                signupPassword.text = "";
                signupConfirm.text = "";
                Debug.Log("existed");
                return;
            }
          
            if(signupConfirm.text!=signupPassword.text)
            {
                messages[5].text = "Confirmation incorrect";
                return;
            }
            if(signupEmail.text.Split('@').Length > 2)
            {
                messages[6].text = "Email not valid";
                return;
            }

            playerlist.Add(new PlayerData(signupUser.text, signupPassword.text, signupEmail.text, "PL" + Guid.NewGuid().ToString().Substring(0, 4)));
            messages[6].text = "";
            messages[7].text = "";
            signupPassword.text = "";
            signupConfirm.text = "";
            Function.Saveinfo<PlayerData>(playerlist);
            showNotification("Account created successfully", false);
            RunLogin();
        }
        else
        {
            if (signupEmail.text.Split('@').Length > 2)
            {
                messages[6].text = "Email not valid";            
            }
            if (signupUser.text.Length < 6)
            {
                messages[7].text = "Must contain atleast 6 characters";
            }
            if (signupPassword.text.Length < 6)
            {
                messages[5].text = "Must contain atleast 6 characters";
            }
            else
            {
                if (signupPassword.text.Length < 6 && signupUser.text.Length < 6)
                {
                    messages[7].text = "Must contain atleast 6 characters";
                    messages[5].text = "Must contain atleast 6 characters";
                }
            }
            return;
        }
        //Dang ki
    }
    //Tu dong dien @gmail.com khi dang ki
    public void Emailsignup(string text)
    {
        string mail = "@gmail.com";
        if (!signupEmail.text.EndsWith(mail))
        {
            for (int i = 1; i < mail.Length; i++)
            {
                if (signupEmail.text.EndsWith(mail.Substring(0, i)))
                {
                    signupEmail.text += mail.Substring(i);
                    break;
                }
            }
            if (!signupEmail.text.EndsWith(mail))
            {
                signupEmail.text += mail;
            }
        }
    }
    //Hien thi thong bao
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
    //Hien thi tin nhan thong bao va tat sau vai giay
    IEnumerator ShowMessage(string message,int i,float delay)
    {
        messages[i].text=message;
        yield return new WaitForSeconds(delay);
        messages[i].text = "";
    }
    //Dong form thong bao
    public void Noti_button()
    {
        notification.SetActive(false);   
    }
    
    //Chon nut de phu hop voi thong bao(yes/no hoac ok)
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
    //Thoat
    public void Exit()
    {       
        showNotification("Exit game?",true);
        Debug.Log("exit");
        buttons[1].onClick = new Button.ButtonClickedEvent();
        buttons[1].onClick.AddListener(Application.Quit);
    }
    //Dang xuat
    public void Logout()
    {         
        showNotification("Are you sure you want to log out ",true);
    }
    //Bat dau choi
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    //Up anh tu file explorer
    public void GetImage()
    {
        StartCoroutine(GetTexture());
    }
    IEnumerator GetTexture()
    {
        string path = EditorUtility.OpenFilePanel("Choose picture", "", "jpg,jpeg,png");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
    //Chuyen anh thanh string de luu va nguoc lai
    //#
    public string Imagetostring(RawImage image)
    {
        Texture2D texture =  image.texture as Texture2D;     
        byte[] bytes = texture.EncodeToPNG();
        string ItoStr = Convert.ToBase64String(bytes);  
        return ItoStr; 
    }
    public RawImage stringtoImage(string text)
    {
        byte[] bytes = Convert.FromBase64String(text);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);        
        image.texture = texture;
        return image;
    }
    //#

    private void Start()
    {
        playerlist = Function.Readinfo<PlayerData>();
        signupEmail.onEndEdit.AddListener(Emailsignup);
        RunLogin();
    }
    
}
