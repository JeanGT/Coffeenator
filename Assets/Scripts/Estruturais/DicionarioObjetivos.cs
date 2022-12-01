using System.Collections;
using System.Collections.Generic;

public static class DicionarioObjetivos {
    private static ArrayList objetivos = new ArrayList();

    static DicionarioObjetivos() {
        //Nome, quantidade, é principal, progressso ao iniciar, progresso ao terminar
        //-1 = não muda o progressso ao terminar
        objetivos.Add(new Objetivo("Comprar um cafézinho", 1, true)); //0
        objetivos.Add(new Objetivo("Treinar no dojo", 1, true)); //1
        objetivos.Add(new Objetivo("Falar com mestre Orochi", 1, true)); //2
        objetivos.Add(new Objetivo("Dormir", 1, true)); //3
        objetivos.Add(new Objetivo("Encontrar pistas sobre o sumiço do café", 1, true)); //4
        objetivos.Add(new Objetivo("Derrotar aprendizes do Orochi", 3, true)); //5
        objetivos.Add(new Objetivo("Derrotar mestre Orochi", 1, true)); // 6
        objetivos.Add(new Objetivo("Encontrar as chaves", 3, true)); // 7
        objetivos.Add(new Objetivo("Conseguir a senha do wi-fi", 1, true)); //8
        objetivos.Add(new Objetivo("Entregar a senha para o Bob", 1, true)); //9
        objetivos.Add(new Objetivo("Entrar no armazém", 1, true)); //10
    }

    public static Objetivo getObjetivoByIndex(int index) {
        return new Objetivo(((Objetivo)objetivos[index]).getContent(), ((Objetivo)objetivos[index]).getMax(), true);
    }

    public static int getIndexByObjetivo(Objetivo obj) {
        for(int i = 0; i < objetivos.Count; i++)
        {
            if (((Objetivo)objetivos[i]).getContent().Equals(obj.getContent()))
            {
                return i;
            }
        }
        return -1;
    }
}
