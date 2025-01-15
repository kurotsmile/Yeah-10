using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Question_Number : MonoBehaviour
{
    [Header("Main")]
    public Game game;

    [Header("Question Obj")]
    public GameObject panel_alert;
    public Text txt_return_alert;
    public Text txt_timer;
    public Color32 color_bk_n1;
    public Color32 color_bk_n2;
    public Image img_alert;
    public Sprite sp_alert_yeah_10;
    public Sprite sp_alert_same;
    public bool is_alert;

    private int count_question = 0;
    private Num_Obj n_1 = null;
    private Num_Obj n_2 = null;

    private float secondsCount;
    private int minuteCount;
    private int hourCount;
    private bool is_play = true;

    public void load_data()
    {
        this.restart();
    }

    public void restart()
    {
        this.panel_alert.SetActive(false);
        this.n_1 = null;
        this.n_2 = null;
        this.is_play = true;
        this.secondsCount = 0;
        this.minuteCount = 0;
        this.hourCount = 0;
    }

    private void Update()
    {
        if(this.is_play) UpdateTimerUI();
    }

    public void UpdateTimerUI()
    {
        secondsCount += Time.deltaTime;
        txt_timer.text = string.Format("{0:00}:{1:00}:{2:00}", hourCount, minuteCount, secondsCount);

        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            hourCount++;
            minuteCount = 0;
        }
    }

    public void add_numb_obj(Num_Obj nun_obj)
    {
        this.count_question++;
        if (this.count_question == 1)
        {
            this.n_1 = nun_obj;
            this.n_1.img_bk.color = this.color_bk_n1;
            this.n_1.GetComponent<Button>().interactable = false;
            this.n_1.txt_show.color = Color.black;
        }

        if (this.count_question == 2)
        { 
            this.n_2 = nun_obj;
            this.n_2.img_bk.color = this.color_bk_n2;
            this.n_2.GetComponent<Button>().interactable = false;
            this.n_2.txt_show.color = Color.black;
        }

        if (this.n_1 != null && this.n_2 != null)
        {
            bool true_check = false;
            if (this.n_1.row_num == this.n_2.row_num) {
                true_check=this.n_1.check_row_true(this.n_2);
                if (true_check == false) true_check = game.check_row_space_number_true(this.n_1.row_num, this.n_1.col_num, this.n_2.col_num);
            }

            if (this.n_1.col_num == this.n_2.col_num){
                true_check=this.n_1.check_col_true(this.n_2);
                if (true_check == false) true_check = game.check_col_space_number_true(this.n_1.row_num, this.n_1.col_num, this.n_2.col_num);
            }

            if (true_check) {
                if (this.n_1.int_num + this.n_2.int_num == 10)
                    this.show_select_true(true);
                else if (this.n_1.int_num == this.n_2.int_num)
                    this.show_select_true(false);
                else
                   StartCoroutine(Reset_select_false());
            }
            else
            {
                StartCoroutine(Reset_select_false());
            }
        }
    }

    private void show_select_true(bool is_yeah_10)
    {
        if (is_yeah_10) {
            this.img_alert.sprite = this.sp_alert_yeah_10;
            this.txt_return_alert.text = this.n_1.int_num + " + " + this.n_2.int_num + " = 10";
            game.carrot.play_vibrate();
        }
        else
        {
            this.img_alert.sprite = this.sp_alert_same;
            this.txt_return_alert.text = this.n_1.int_num + " = " + this.n_2.int_num;
        }

        this.n_1.is_show=false;
        this.n_2.is_show = false;

        game.set_pos_effect_bee_true(this.n_1.transform.position, this.n_2.transform.position, is_yeah_10);
        if (this.is_alert) this.panel_alert.SetActive(true);
        StartCoroutine(reset_select_true());
    }

    IEnumerator Reset_select_false()
    {
        yield return new WaitForSeconds(0.5f);
        this.n_1.reset();
        this.n_2.reset();
        this.n_1 = null;
        this.n_2 = null;
        this.count_question = 0;
        game.play_sound(2);
    }

    IEnumerator reset_select_true()
    {
        yield return new WaitForSeconds(0.5f);
        this.n_1.deactivate();
        this.n_2.deactivate();
        this.n_1 = null;
        this.n_2 = null;
        this.count_question = 0;
        game.stop_effect_bee_true();
        this.panel_alert.SetActive(false);
    }

    public void reset_select()
    {
        if (this.n_1 != null)
        {
            this.n_1.reset();
            this.n_1 = null;
        }
        
        if(this.n_2 != null)
        {
            this.n_2.reset();
            this.n_2 = null;
        }
        this.count_question = 0;
    }

    public void pause()
    {
        this.is_play = false;
    }

    public void unPause()
    {
        this.is_play = true;
    }
}
