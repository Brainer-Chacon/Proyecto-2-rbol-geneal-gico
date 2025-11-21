using System.Collections.Generic;
using ArbolGenealogico.Domain;


public class ModuloEstadisticas 
{

    public static (string, string, double) ParMasCerca(List<(string name1 , string name2 , double dis)> distancias)
    {
        (string name1 , string name2 , double dis) answer =  distancias[0];

        for(int i = 0; i < distancias.Count; i++)
        {
            if(answer.dis > distancias[i].dis)
            {
                answer = distancias[i];
            }
        }
        return answer;
    }

    public static (string, string, double) ParMasLejos(List<(string name1 , string name2 , double dis)> distancias)
    {
        (string name1 , string name2 , double dis) answer =  (null, null, 0);

        for(int i = 0; i < distancias.Count; i++)
        {
            if(answer.dis <= distancias[i].dis)
            {
                answer = distancias[i];
            }
        }
        return answer;
    }

    public static double Promedio(List<(string name1 , string name2 , double dis)> distancias)
    {
        double answer = 0;

        for(int i = 0; i < distancias.Count; i++)
        {
            answer += distancias[i].dis;
        }

        return answer/distancias.Count;
    }

}
