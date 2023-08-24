using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Num_Obj : MonoBehaviour
{
    public Text txt_show;
    public Image img_bk;
    public int int_num;
    public int col_num;
    public int row_num;
    public bool is_show = true;
    private bool is_select = false;
    public Animator ani;

    public void load_data()
    {
        this.GetComponent<Button>().interactable = false;
    }

    public void click()
    {
        if (this.is_select == false)
        {
            GameObject.FindAnyObjectByType<Game>().add_num_obj_to_question(this);
            this.is_select = true;
        }
    }

    public bool check_row_true(Num_Obj n)
    {
        if(n.col_num == col_num + 1) return true;
        if(n.col_num == col_num - 1) return true;
        return false;
    }

    public bool check_col_true(Num_Obj n)
    {
        if (n.row_num == row_num + 1) return true;
        if (n.row_num == row_num - 1) return true;
        return false;
    }

    public void deactivate()
    {
        this.is_show = false;
        this.txt_show.gameObject.SetActive(false);
        this.img_bk.color = Color.yellow;
        this.GetComponent<Button>().interactable = false;
    }

    public void select()
    {
        if (this.is_show) { 
            this.txt_show.color = Color.white;
            this.img_bk.color = Color.grey;
            this.GetComponent<Button>().interactable = false;
        }
    }

    public void gamepad_select()
    {
        if (this.is_show)
        {
            if (this.is_select == false)
            {
                this.txt_show.color = Color.white;
                this.img_bk.color = Color.blue;
            }
        }
        else
        {
            this.img_bk.color = Color.blue;
        }
    }

    public void gamepad_UnSelect()
    {
        if (this.is_show)
        {
            if (this.is_select == false) { 
                this.txt_show.color = Color.white;
                this.img_bk.color = Color.black;
            }
        }
        else
        {
            this.txt_show.gameObject.SetActive(false);
            this.img_bk.color = Color.yellow;
        }
    }

    public void reset()
    {
        this.is_select = false;
        this.img_bk.color = Color.black;
        this.txt_show.color = Color.white;
        this.GetComponent<Button>().interactable = true;
    }

    public void suggest()
    {
        this.ani.enabled = true;
        this.GetComponent<Button>().interactable = false;
    }

    public void stop_suggest()
    {
        this.GetComponent<Button>().interactable = true;
        this.ani.enabled = false;
    }
}
