using System.Collections;
using System.Collections.Generic;

public class Objetivo 
{
    private string content;
    private int max;
    private int current;
    private bool primario;

    public Objetivo(string content, int max, bool primario) {
        this.content = content;
        this.max = max;
        this.primario = primario;
    }

    public string getContent() {
        return content;
    }

    public int getMax() {
        return max;
    }

    public int getCurrent() {
        return current;
    }

    public bool isPrincipal() {
        return this.primario;
    }

    public void foward(int steps) {
        current += steps;
        if(current >= max) {
            end();
        }
    }

    private void end() {
        PlayerStatus.removeObjective(this);
    }
}
