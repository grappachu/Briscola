namespace Grappachu.Briscola.Model
{
    // Step 1: Crea una card come struct. E' più efficiente ed essendo passata per valore 
    //         non mi serve implementare l'equals (vedi test sulla creazione del mazzo)
    public struct Card
    {
        public Card(string seed, int value)
        {
            Seed = seed;
            Value = value;
        }

        public string Seed { get; }
        public int Value { get; }

        public override string ToString()
        {
            return string.Format("{0}-{1:00}", Seed, Value);
        }
    }
}