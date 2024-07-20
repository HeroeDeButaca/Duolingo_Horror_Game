[System.Serializable]
public class DatosJuego
{
    public bool[] cartasObtenidas = new bool[9];
    public bool[] nochesSuperadas = new bool[10];
    public DatosJuego(GameOver gameOver)
    {
        nochesSuperadas = gameOver.nochesSuperadas;
        cartasObtenidas = gameOver.cartasObtenidas;
    }
}
