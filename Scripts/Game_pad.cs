using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_pad : MonoBehaviour
{
    public Carrot.Carrot carrot;

    private int gamepad_x = 0;
    private int gamepad_y = 0;

    private Carrot.Carrot_Gamepad gamepad_1;
    private bool is_gamepad_play = true;

    public void load_gamepad()
    {
        this.gamepad_1 = this.carrot.game.create_gamepad("player_1");
        this.gamepad_1.set_gamepad_keydown_left(gampad_keydown_left);
        this.gamepad_1.set_gamepad_keydown_right(gampad_keydown_right);
        this.gamepad_1.set_gamepad_keydown_down(gampad_keydown_down);
        this.gamepad_1.set_gamepad_keydown_up(gampad_keydown_up);
        this.gamepad_1.set_gamepad_keydown_select(gampad_keydown_select);
        this.gamepad_1.set_gamepad_keydown_start(gamepad_keydown_start);
        this.gamepad_1.set_gamepad_keydown_a(gamepad_keydown_a);
        this.gamepad_1.set_gamepad_keydown_x(gamepad_keydown_x);
        this.gamepad_1.set_gamepad_keydown_y(gamepad_keydown_y);
        this.gamepad_1.set_gamepad_keydown_b(gamepad_keydown_b);

        this.set_yes_gamepad_play();
    }

    public void btn_show_gamepad_setting()
    {
        this.gamepad_1.show_setting_gamepad();
    }

    private void gampad_keydown_left()
    {
        if (this.is_gamepad_play)
        {
            this.reset_all_row_gamepad();
            this.gamepad_x--;
            if (this.gamepad_x < 0) this.gamepad_x = 9;
            this.GetComponent<Game>().get_list_row()[this.gamepad_y].get_num_by_col(this.gamepad_x).gamepad_select();
        }
        else
        {
            this.carrot.game.gamepad_keydown_up_console();
        }
    }

    private void gampad_keydown_right()
    {
        if (this.is_gamepad_play)
        {
            this.reset_all_row_gamepad();
            this.gamepad_x++;
            if (this.gamepad_x > 9) this.gamepad_x = 0;
            this.GetComponent<Game>().get_list_row()[this.gamepad_y].get_num_by_col(this.gamepad_x).gamepad_select();
        }
        else
        {
            this.carrot.game.gamepad_keydown_down_console();
        }

    }

    private void gampad_keydown_down()
    {
        if (this.is_gamepad_play)
        {
            this.reset_all_row_gamepad();
            this.gamepad_y--;
            if (this.gamepad_y < 0) this.gamepad_y = this.GetComponent<Game>().get_list_row().Count - 1;
            this.GetComponent<Game>().get_list_row()[this.gamepad_y].get_num_by_col(this.gamepad_x).gamepad_select();
        }
        else
        {
            this.carrot.game.gamepad_keydown_down_console();
        }

    }

    private void gampad_keydown_up()
    {
        if (this.is_gamepad_play)
        {
            this.reset_all_row_gamepad();
            this.gamepad_y++;
            if (this.gamepad_y >= this.GetComponent<Game>().get_list_row().Count) this.gamepad_y = 0;
            this.GetComponent<Game>().get_list_row()[this.gamepad_y].get_num_by_col(this.gamepad_x).gamepad_select();
        }
        else
        {
            this.carrot.game.gamepad_keydown_up_console();
        }
    }

    private void gampad_keydown_select()
    {
        if (this.is_gamepad_play)
        {
            this.btn_select();
        }
        else
        {
            this.carrot.game.gamepad_keydown_enter_console();
        }
        
    }

    private void gamepad_keydown_a()
    {
        if (this.is_gamepad_play)
        {
            this.btn_select();
        }
        else
        {
            this.carrot.game.gamepad_keydown_enter_console();
        }
        
    }
    
    private void gamepad_keydown_x()
    {
        if (this.is_gamepad_play)
        {
            this.GetComponent<Game>().btn_add_rows();
        }
        else
        {
            this.carrot.game.gamepad_keydown_enter_console();
        }
    }

    private void gamepad_keydown_b()
    {
        if (this.is_gamepad_play)
        {
            if (this.GetComponent<Game>().obj_button_tool.activeInHierarchy)
            {
                this.GetComponent<Game>().btn_active_tool();
            }
            else
            {
                this.GetComponent<Game>().btn_show_shop();
            }
        }
        else
        {
            this.carrot.game.gamepad_keydown_enter_console();
        }
    }

    private void gamepad_keydown_y()
    {
        if (this.is_gamepad_play)
            this.GetComponent<Game>().btn_show_setting();
        else
            this.carrot.game.gamepad_keydown_enter_console();
    }

    private void gamepad_keydown_start()
    {
        if(this.is_gamepad_play)
            this.GetComponent<Game>().btn_game_replay();
        else
            this.carrot.game.gamepad_keydown_enter_console();
    }

    private void btn_select()
    {
        Num_Obj n_obj = this.GetComponent<Game>().get_list_row()[this.gamepad_y].get_num_by_col(this.gamepad_x);
        if (n_obj.GetComponent<Button>().interactable) n_obj.click();
    }

    private void reset_all_row_gamepad()
    {
        for (int i = 0; i < this.GetComponent<Game>().get_list_row().Count; i++) this.GetComponent<Game>().get_list_row()[i].gamepad_UnSelect();
    }

    public void set_no_gamepad_play()
    {
        this.carrot.game.set_enable_gamepad_console(true);
        this.is_gamepad_play = false;
    }

    public void set_yes_gamepad_play()
    {
        this.carrot.game.set_enable_gamepad_console(false);
        this.is_gamepad_play = true;
    }

    public void reset_gamepad()
    {
        this.gamepad_x = 0;
        this.gamepad_y = 0;
    }
}
