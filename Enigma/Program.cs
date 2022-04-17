/*
 * El alfabeto consiste en letras y espacio para que se puedan cifrar oraciones completas
 * Si se desea cifrar también con números es necesario agregarlos a cada uno de los alfabetos,
 * mismo caso con los signos de puntuación y cualquier otro que deseen.
 */
string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";

//Se pueden usar diferentes orden en cada uno de los diccionarios
Dictionary<string, string> rotor1 = new Dictionary<string, string>();
string alfabetoR1 = "EFGPXYZAQB CDRSTUVWHIJKLMNO";
Dictionary<string, string> rotor2 = new Dictionary<string, string>();
string alfabetoR2 = "PBCDEFSTUVWQOXYZR MNGHIAKLJ";
Dictionary<string, string> rotor3 = new Dictionary<string, string>();
string alfabetoR3 = "GMNORS TUVHIJKLWXYZABPQCDEF";

for(int i = 0; i < alfabeto.Length; i++) {
    rotor1.Add(alfabeto[i].ToString(), alfabetoR1[i].ToString());
    rotor2.Add(alfabeto[i].ToString(), alfabetoR2[i].ToString());
    rotor3.Add(alfabeto[i].ToString(), alfabetoR3[i].ToString());
}

Console.Write("Ingrese el mensaje: ");
string mensajeOriginal = Console.ReadLine().ToUpper();

//Solo se tomarán los primeros digitos de la clave que introduzcan
Console.Write("Ingrese la clave (3 dígitos): ");
string clave = Console.ReadLine().ToUpper();

string mensajeCifrado = Cifrar(mensajeOriginal, clave);
Console.WriteLine("\nMensaje Cifrado: " + mensajeCifrado);

string mensajeDescifrado = Descifrar(mensajeCifrado, clave);
Console.WriteLine("\nMensaje Descifrado: " + mensajeDescifrado);

string Cifrar(string mensajeOriginal, string clave_) {
    AjustarRotores(clave_);
    string mensajeCifrado_ = "";
    int cantidadLetrasAlfabeto = 0;
    for(int i = 0; i < mensajeOriginal.Length; i++) {
        mensajeCifrado_ += rotor1[rotor2[rotor3[mensajeOriginal[i].ToString()]]];
        rotor3 = RotarRotor(rotor3);
        cantidadLetrasAlfabeto++;
        if(cantidadLetrasAlfabeto == alfabeto.Length) {
            cantidadLetrasAlfabeto = 0;
            rotor2 = RotarRotor(rotor2);
        }
    }
    return mensajeCifrado_;
}

string Descifrar(string mensajeCifrado_, string clave_) {
    AjustarRotores(clave_);
    string mensajeDescifrado_ = "";
    int cantidadLetrasAlfabeto = 0;
    for(int i = 0; i < mensajeCifrado_.Length; i++) {
        string key1 = rotor1.Where(x => x.Value.Contains(mensajeCifrado_[i])).Select(x => x.Key).FirstOrDefault();
        string key2 = rotor2.Where(x => x.Value.Contains(key1)).Select(x => x.Key).FirstOrDefault();
        string key3 = rotor3.Where(x => x.Value.Contains(key2)).Select(x => x.Key).FirstOrDefault();
        mensajeDescifrado_ += key3;
        rotor3 = RotarRotor(rotor3);
        cantidadLetrasAlfabeto++;
        if(cantidadLetrasAlfabeto == alfabeto.Length) {
            cantidadLetrasAlfabeto = 0;
            rotor2 = RotarRotor(rotor2);
        }
    }
    return mensajeDescifrado_;
}

#region MovimientosRotores
Dictionary<string, string> RotarRotor(Dictionary<string, string> rotor) {
    Dictionary<string, string> rotorTemp = new Dictionary<string, string>();
    for(int i = 0; i < rotor.Count; i++) {
        try {
            rotorTemp.Add(alfabeto[i].ToString(), rotor.ElementAt(i + 1).Value);
        } catch {
            rotorTemp.Add(alfabeto[i].ToString(), rotor.ElementAt(0).Value);
        }
    }
    return rotorTemp;
}

Dictionary<string, string> AjustarRotor(Dictionary<string, string> rotor, string clave_) {
    Dictionary<string, string> rotorTemp = new Dictionary<string, string>();
    for(int i = 0; i < alfabeto.Length; i++)
        rotorTemp.Add(alfabeto[i].ToString(), rotor.ElementAt(i).Value);
    do {
        rotorTemp = RotarRotor(rotorTemp);
    } while(!rotorTemp.ElementAt(0).Value.Contains(clave_));
    return rotorTemp;
}

void AjustarRotores(string clave_) {
    rotor1 = AjustarRotor(rotor1, clave_[0].ToString());
    rotor2 = AjustarRotor(rotor2, clave_[1].ToString());
    rotor3 = AjustarRotor(rotor3, clave_[2].ToString());
}
#endregion