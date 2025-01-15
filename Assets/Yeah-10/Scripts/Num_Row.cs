using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Num_Row : MonoBehaviour
{
    public GameObject Number_obj_prefab;
    public Transform tr_all_num_obj;
    private System.Random _random = new System.Random();

    //private int[] num_int = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public int row_num = 0;
    private List<Num_Obj> list_num;

    public void load_data()
    {
        this.list_num = new List<Num_Obj>();
        for(int i = 0; i < 10; i++)
        {
            GameObject num_obj = Instantiate(this.Number_obj_prefab);
            num_obj.transform.SetParent(this.tr_all_num_obj);
            num_obj.transform.localScale = new Vector3(1f, 1f, 1f);
            int int_show_rand= Random.Range(1,10);
            num_obj.GetComponent<Num_Obj>().txt_show.text = int_show_rand.ToString();
            num_obj.GetComponent<Num_Obj>().int_num = int_show_rand;
            num_obj.GetComponent<Num_Obj>().col_num = i;
            num_obj.GetComponent<Num_Obj>().row_num = this.row_num;
            num_obj.GetComponent<Num_Obj>().load_data();
            this.list_num.Add(num_obj.GetComponent<Num_Obj>());
        }
    }

    public bool check_space_true(int index_col_n1,int index_col_n2)
    {
        bool is_space_row = true;
        if (index_col_n2 > index_col_n1)
        {
            for(int i= index_col_n1+1;i< index_col_n2; i++) if (this.list_num[i].is_show != false) is_space_row = false;
        }
        else
        {
            for (int i = index_col_n2+1; i < index_col_n1; i++) if (this.list_num[i].is_show != false) is_space_row = false;
        }
        return is_space_row;
    }

    public bool check_show_number_col(int index_col)
    {
        return this.list_num[index_col].is_show;
    }

    public Num_Obj get_num_by_col(int index_col)
    {
        return this.list_num[index_col];
    }

    public bool check_win_game()
    {
        for(int i = 0; i < this.list_num.Count; i++) if (this.list_num[i].is_show) return false;
        return true;
    }

    public void deactivate()
    {
        for (int i = 0; i < this.list_num.Count; i++) this.list_num[i].deactivate();
    }

    public void select()
    {
        for (int i = 0; i < this.list_num.Count; i++) this.list_num[i].select();
    }

    public void select_by_int(int num_int)
    {
        for(int i = 0; i < this.list_num.Count; i++) if (this.list_num[i].int_num == num_int) this.list_num[i].select();
    }

    public void deactivate_by_int(int num_int)
    {
        for (int i = 0; i < this.list_num.Count; i++) if (this.list_num[i].int_num == num_int) this.list_num[i].deactivate();
    }

    public void effect_boom_by_int(int num_int)
    {
        for (int i = 0; i < this.list_num.Count; i++) 
        if (this.list_num[i].int_num == num_int)
        {
            if (this.list_num[i].is_show) GameObject.FindAnyObjectByType<Game>().add_effect_boom(this.list_num[i].transform.position);
        }
    }

    public bool suggest()
    {
        bool is_true = false;
        for(int i = 0; i < this.list_num.Count; i++)
        {
            if(this.list_num[i].is_show)
            for (int y = i+1; y < this.list_num.Count; y++)
            {
                    if ((this.list_num[i].int_num == this.list_num[y].int_num) && (i + 1) == y&&this.list_num[y].is_show==true)
                    {
                        this.list_num[i].suggest();
                        this.list_num[y].suggest();
                        Debug.Log("co");
                        is_true = true;
                        break;
                    }

                    if ((this.list_num[i].int_num + this.list_num[y].int_num==10) && (i + 1) == y && this.list_num[y].is_show == true&&is_true==false)
                    {
                        this.list_num[i].suggest();
                        this.list_num[y].suggest();
                        Debug.Log("co");
                        is_true = true;
                        break;
                    }
                }
            if (is_true) break;
        }
        return is_true;
    }

    public void gamepad_UnSelect()
    {
        for (int i = 0; i < this.list_num.Count; i++) this.list_num[i].gamepad_UnSelect();
    }
}
