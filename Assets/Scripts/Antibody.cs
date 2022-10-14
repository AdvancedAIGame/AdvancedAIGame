// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomStrings {
    class Antibody {
        //To genrate random numbers
        private Random random = new Random();
        private const string cell = "abcdefghijklmnopqrstuvwxyz";
        private int cell_len = 0;
        //constructor 
        public Antibody(int length){
            cell_len = length;
        }

        //function to generate random strings
        public string generate_string(){
            return new string(Enumerable.Repeat(cell, cell_len).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //function for matching for affinity of player and enemy virus signatures using the Levenshtein distance algorithm
        public int ComputeAffinity(string s, string t){
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];//creating array
        
            // Verify arguments.
            if (n == 0)
                return m;
            else if (m == 0)
                return n;
        
            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++){
            }
        
            for (int j = 0; j <= m; d[0, j] = j++){
            }
        
            // Begin looping.
            for (int i = 1; i <= n; i++){
                for (int j = 1; j <= m; j++){
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }

        //entry point
        public static void Main(string[] args){
            Antibody at = new Antibody(12);
            string finalString = at.generate_string();
            Console.WriteLine("----------Generated String Signature for antibody---------");
            Console.WriteLine(finalString);
            Console.WriteLine();

            Console.WriteLine("------------Creating a random Population of antibodies------------");
            List<string> p = at.population(4);
            Console.WriteLine("Population size = 4");
            p.ForEach(item =>
               Console.WriteLine(item)
            );
            Console.WriteLine();
            
            Console.WriteLine("-----------Creating clones for antibody-------------------");
            List<string> clones = at.clone("tdxsyqchiwpr", 5);
            Console.WriteLine("antibody: tdxsyqchiwpr");
            Console.WriteLine("clones----");
            clones.ForEach(item =>
               Console.WriteLine(item)
            );
            Console.WriteLine();

            Console.WriteLine("----------------Computing Affinity between antibody and antigen ----------------------");
            string anti = "jdumeyrbqwzp";
            string anti2 = "cfmkijqsbalu";
            Console.WriteLine("antibody: jdumeyrbqwzp");
            Console.WriteLine("Antigen: cfmkijqsbalu");
            Console.WriteLine("affinity ="+at.ComputeAffinity(anti2,anti));
            Console.WriteLine();

            Console.WriteLine("----------------------HyperMuatating the antibody----------------------------------");
            Console.WriteLine("before mutation: ftzvcnlgiorq");
            Console.WriteLine("after mutation: " + at.hypermutate("ftzvcnlgiorq", 7));
            Console.WriteLine();


            //getting the best 
            Console.WriteLine("----------------------Selecting the best out of the population----------------------");
            Console.WriteLine("--Original population-----");
               string anti3 = "abcdefghijkl";
            p.ForEach(item =>
               Console.WriteLine(item + "," + at.ComputeAffinity(item,anti3))
            );
            Console.WriteLine();

            Console.WriteLine("--Selected from Population---");
            var tupleList = at.select(p, anti3, 2);
            tupleList.ForEach(item =>
               Console.WriteLine(item  + "," + at.ComputeAffinity(item,anti3))
            );
            Console.WriteLine();

            Console.WriteLine("----------------------Replacing the worst in population----------------------------");
            Console.WriteLine("--Original Population----");
            p.ForEach(item =>
               Console.WriteLine(item)
            );
            Console.WriteLine();
            
            Console.WriteLine("--Population After worst has been replaced---");
            var tuplList = at.replace(p,tupleList);
            tuplList.ForEach(item =>
                Console.WriteLine(item)
            );
        }

        //function to convert string  to array of characters
        public char[] convert_to_list(string s){
            return s.ToCharArray();
        }

        //function to populate the list with a number of cells
        public List<string> population(int pop_size){
            List<string> pop = new List<string>();
            for(int p = 0;p < pop_size;p++){
                String co = generate_string();
                pop.Add(co);
            } 
            return pop;
        } 

        //function to clone 
        public List<string> clone(string p,int pa){
            int clone_num = p.Length - pa;
            List<string> clones = new List<string>();
            for(int pp = 0;pp < clone_num;pp++){
                clones.Add(p);
            }
            return clones;
        }

        //function to hypermutate
        public string hypermutate(string p,int ap){
            char[] pp = convert_to_list(p);
            Random rand = new Random();

            for(int r = 0;r < ap;r++){
                int index = rand.Next(p.Length);
                char[] chcells = convert_to_list(cell);
                int c = rand.Next(chcells.Length);
                pp[index] = chcells[c];
            }


            string mutated_p = "";
            for(int r = 0;r < p.Length;r++){
                mutated_p += pp[r];
            }

            return mutated_p;
        }

        //function to select the best population
        public List<string> select(List<string> pop,string antigen,int size){
            List<(int, string)> ranked_pop = new List<(int, string)>();
            for(int p = 0;p < pop.Count ;p++){
                int aff =  ComputeAffinity(pop[p],antigen);
                List<string> clon = clone(pop[p],aff);
                pop.AddRange(clon);
            }

            for(int p = 0;p < pop.Count ;p++){
                int aff =  ComputeAffinity(pop[p],antigen);
                ranked_pop.Add((aff,pop[p]));
            }

            ranked_pop.Sort();
            List<string> ranked_p = new List<string>();
            for(int p = 0;p < size ;p++){
                ranked_p.Add(ranked_pop[p].Item2);
            }

            return ranked_p;
        }

        //function to replace
        public List<string> replace(List<string> pop,List<string> selected){
            int pop_len = pop.Count - selected.Count;
            List<string> new_pop = new List<string>();

            foreach (string a in selected){
                new_pop.Add(a);
            }

            for(int p = 0;p < pop_len ;p++){
                string antib  = generate_string();
                new_pop.Add(antib);
            }

            return new_pop;
        }
    }
}