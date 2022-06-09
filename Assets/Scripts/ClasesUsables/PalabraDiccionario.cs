using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*elemento no usado queriamos probar algo*/
public struct PalabraDiccionario
{
    public PalabraDiccionario(string palabra, int id)
    {
        Palabra = palabra;
        Id = id;
    }
    public string Palabra { get; }
    public int Id { get; }

    public override string ToString() => $"{Palabra} | {Id}";
}
