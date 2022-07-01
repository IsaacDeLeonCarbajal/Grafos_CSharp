using System;
using System.Collections.Generic;

namespace Grafos
{
    class Program
    {
        static void Main(string[] args)
        {
            Grafo grafo = new Grafo();

            grafo.InsertarVertice('A');
            grafo.InsertarVertice('B');
            grafo.InsertarVertice('C');
            grafo.InsertarVertice('D');
            grafo.InsertarVertice('E');
            grafo.InsertarArista(new char[] { 'A', 'B' });
            grafo.InsertarArista(new char[] { 'A', 'E' });
            grafo.InsertarArista(new char[] { 'B', 'C' });
            grafo.InsertarArista(new char[] { 'B', 'E' });
            grafo.InsertarArista(new char[] { 'C', 'C' });
            grafo.InsertarArista(new char[] { 'C', 'E' });
            grafo.InsertarArista(new char[] { 'E', 'D' });
            grafo.InsertarArista(new char[] { 'D', 'E' });
            
            int opc;
            bool error;
            
            do {
                Console.Write("\n\n\n\n\nGRAFOS\n" +
                "1 Insertar Vértice\n" +
                "2 Insertar Arista\n" +
                "3 Eliminar Vértice\n" +
                "4 Eliminar Arista\n" +
                "5 Matriz de Incidencia\n" +
                "6 Matriz de Adyacencia\n" +
                "7 Ciclico\n" +
                "8 Buscar Camino\n" +
                "9 Salir\n\n" +
                "OPCION: ");
                
                try {
                    opc = Int32.Parse(Console.ReadLine());
                } catch {
                    Console.WriteLine("Ingrese una opción válida");
                    opc = 0;
                    continue;
                }
                
                error = false;
                
                switch (opc) {
                    case 1:
                        Console.WriteLine("Insertar Vértice");
                        error = (grafo.InsertarVertice(Grafo.PedirValor()) == null);
                        break;
                    case 2:
                        Console.WriteLine("Insertar Arista");
                        error = (grafo.InsertarArista(new char[] {
                            Grafo.PedirValor(),
                            Grafo.PedirValor()
                        }) == null);
                        break;
                    case 3:
                        Console.WriteLine("Eliminar Vértice");
                        error = !grafo.EliminarVertice(Grafo.PedirValor());
                        break;
                    case 4:
                        Console.WriteLine("Eliminar Arista");
                        error = !grafo.EliminarArista(Grafo.PedirIndice());
                        break;
                    case 5:
                        Console.WriteLine("Matriz de Incidencia");
                        Console.WriteLine(grafo.MostrarMatriz(grafo.CrearMatrizIncidencia(), 'I'));
                        break;
                    case 6:
                        Console.WriteLine("Matriz de Adyacencia");
                        Console.WriteLine(grafo.MostrarMatriz(grafo.CrearMatrizAdyacencia(), 'A'));
                        break;
                    case 7:
                        bool ciclico = grafo.EsCiclico(grafo.GetVertice(0), new List<int>());
                        Console.WriteLine("El grafo es " + (ciclico? "Ciclico" : "Aciclico"));
                        break;
                    case 8:
                        Console.WriteLine("Buscar Camino");
                        Vertice[] vs = {grafo.GetVertice(Grafo.PedirValor()), grafo.GetVertice(Grafo.PedirValor())};
                        List<int> camino = grafo.BuscarCamino(vs[0], vs[1], new List<int>());
                        camino.Reverse();
                        
                        Console.WriteLine("Camino: ");
                        
                        foreach (int i in camino) {
                            Console.Write(grafo.GetVertice(i).Valor + " - ");
                        }
                        break;
                    case 9:
                        Console.WriteLine("SALIR");
                        break;
                }
                
                if (opc < 1 || opc > 9) {
                    Console.WriteLine("\nOpción Inválida");
                } else if (error) {
                    Console.WriteLine("\nOcurrió un Error");
                }
            } while (opc != 9);
            
            Console.Write("\n\n\n\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    class Grafo
    {
        private List<Vertice> _vertices;
        private List<Arista> _aristas;
        
        public static char PedirValor()
        {
            bool error;
            char valor = '-';
            
            do {
                Console.Write("    Valor: ");
                valor = Console.ReadKey().KeyChar;
                
                if (valor >= 97) {
                    valor = (char)(valor - 32);
                }
                
                error = !(valor >= 65 && valor <= 90);
                
                if (error) {
                    Console.WriteLine("Debe ingresar una letra. Ingresado " + valor);
                }
            } while (error);
            
            return valor;
        }
        
        public static int PedirIndice()
        {
            bool error;
            int indice = -1;
            
            do {
                error = false;
                
                try {
                    Console.Write("    Indice: ");
                    indice = Int32.Parse(Console.ReadLine());
                } catch {
                    error = true;
                }
                
                error = error || (indice < 1);
                
                if (error) {
                    Console.WriteLine("Debe ingresar un número mayor que 0. Ingresado " + indice);
                }
            } while (error);
            
            return indice - 1;
        }

        public Grafo()
        {
            this.Vertices = new List<Vertice>();
            this.Aristas = new List<Arista>();
        }

        public Vertice InsertarVertice(char valor)
        {
            this.Vertices.Add(new Vertice(this.Vertices.Count, valor));

            return this.Vertices[this.Vertices.Count - 1];
        }

        public bool EliminarVertice(char valor)
        {
            Vertice v = this.GetVertice(valor);

            if (v.Eliminado) {
                Console.WriteLine("Vértice inválido");
                return false;
            }

            foreach (Arista a in this.Aristas) {
                if (a.Eliminado) {
                    continue;
                }

                if ((v == a.Vertice1) || (v == a.Vertice1)) {
                    this.EliminarArista(a.Indice);
                }
            }

            this.Vertices[v.Indice].Eliminar();

            return true;
        }

        public Arista InsertarArista(char[] valores)
        {
            if (valores.Length != 2) {
                return null;
            }

            Vertice v1 = this.GetVertice(valores[0]);
            Vertice v2 = this.GetVertice(valores[1]);

            if ((v1.Eliminado) || (v2.Eliminado)) {
                Console.WriteLine("Vértices inválidos");
                return null;
            }

            this.Aristas.Add(new Arista(this.Aristas.Count, v1, v2));

            return this.Aristas[this.Aristas.Count - 1];
        }

        public bool EliminarArista(int indice)
        {
            Arista a = this.GetArista(indice);

            if (a.Eliminado) {
                Console.WriteLine("Arista inválida");
                return false;
            }

            this.Aristas[indice].Eliminar();
 
            return true;
        }
        
        public bool EsCiclico(Vertice v, List<int> indicesRecorridos)
        {
            indicesRecorridos.Add(v.Indice);
            
            int cont = 0;
            
            foreach (Arista a in this.Aristas) {
                if (a.Eliminado) {
                    continue;
                }
                
                Vertice v1 = a.Vertice1;
                Vertice v2 = a.Vertice2;
                Vertice sig = null;
                
                if (v1.Indice == v.Indice) {
                    sig = v2;
                } else if (v2.Indice == v.Indice) {
                    sig = v1;
                }
                
                if (sig != null) {
                    bool ciclico = false;
                    
                    if (!indicesRecorridos.Contains(sig.Indice)) {
                        ciclico = this.EsCiclico(sig, indicesRecorridos);
                    } else {
                        cont++;
                    }
                    
                    if (cont >= 2 || ciclico) {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        public List<int> BuscarCamino(Vertice v, Vertice buscado, List<int> indicesRecorridos)
        {
            List<int> indicesCamino = new List<int>();
            indicesRecorridos.Add(v.Indice);
            
            if (v == buscado) {
                indicesCamino.Add(v.Indice);
                
                return indicesCamino;
            }
            
            foreach (Arista a in this.Aristas) {
                if (a.Eliminado) {
                    continue;
                }
                
                Vertice v1 = a.Vertice1;
                Vertice v2 = a.Vertice2;
                Vertice sig = null;
                
                if (v1.Indice == v.Indice) {
                    sig = v2;
                } else if (v2.Indice == v.Indice) {
                    sig = v1;
                }
                
                if (sig != null && !indicesRecorridos.Contains(sig.Indice)) {
                    List<int> indicesAux = this.BuscarCamino(sig, buscado, new List<int>(indicesRecorridos));
                    
                    if (indicesAux.Count != 0 && (indicesCamino.Count == 0 || indicesAux.Count < indicesCamino.Count)) {
                        indicesAux.Add(v.Indice);
                        indicesCamino = indicesAux;
                    }
                }
            }
                            
            return indicesCamino;
        }
        
        public int[, ] CrearMatrizAdyacencia()
        {
            int[, ] matriz = this.CrearMatrizVacia(this.Vertices.Count, this.Vertices.Count);
            
            foreach (Arista a in this.Aristas) {
                if (a.Eliminado) {
                    continue;
                }
                
                Vertice v1 = a.Vertice1;
                Vertice v2 = a.Vertice2;
                
                matriz[v1.Indice, v2.Indice] ++;
                matriz[v2.Indice, v1.Indice] ++;
            }
            
            return matriz;
        }
        
        public int[, ] CrearMatrizIncidencia()
        {
            int[, ] matriz = this.CrearMatrizVacia(this.Aristas.Count, this.Vertices.Count);
            
            for (int i = 0; i < this.Aristas.Count; i++) {
                Arista a = this.Aristas[i];
                
                if (a.Eliminado) {
                    continue;
                }
                
                Vertice v1 = a.Vertice1;
                Vertice v2 = a.Vertice2;
                
                matriz[i, v2.Indice] = 1;
                matriz[i, v1.Indice] = 1;
            }
            
            return matriz;            
        }

        private int[,] CrearMatrizVacia(int ancho, int alto)
        {
            int[,] matriz = new int[ancho, alto];
            
            
            for (int x = 0; x < ancho; x++) {
                for (int y = 0; y < alto; y++) {
                    matriz[x, y] = 0;
                }
            }
            
            return matriz;
        }
        
        public string MostrarMatriz(int[, ] matriz, char tipo)
        {
            string texto = "";
            for (int y = 0; y < matriz.GetLength(1); y++) {
                Vertice v = this.GetVertice(y);
                
                if (v.Eliminado) {
                    continue;
                }
                    
                for (int x = 0; x < matriz.GetLength(0); x++) {
                    if ((tipo == 'I') && (this.GetArista(x).Eliminado)) { //Si es una matriz de incidencia y la arista no existe
                        continue; //Ignorar
                    } else if ((tipo == 'A') && (this.GetVertice(x).Eliminado)) { //Si es una matriz de adyacencia y el vértice no existe
                        continue; //Ignorar
                    }
                    
                    texto += matriz[x, y] + " ";
                }
                
                texto += "\n";
            }
            
            return texto;
        }

        public Vertice GetVertice(int indice)
        {
            return ((indice < this.Vertices.Count) ? this.Vertices[indice] : null);
        }

        public Vertice GetVertice(char valor)
        {
            for (int i = 0; i < this.Vertices.Count; i++) {
                Vertice v = this.GetVertice(i);

                if (!v.Eliminado && v.Valor == valor) {
                    return v;
                }
            }

            return null;
        }

        public Arista GetArista(int indice)
        {
            return ((indice < this.Aristas.Count) ? this.Aristas[indice] : null);
        }

        public List<Vertice> Vertices {
            get { return this._vertices; }
            private set { this._vertices = value; }
        }

        public List<Arista> Aristas {
            get { return this._aristas; }
            private set { this._aristas = value; }
        }
    }

    class Vertice
    {
        private int _indice;
        private char _valor;
        private bool _eliminado = false;

        public Vertice(int indice, char valor)
        {
            this.Indice = indice;
            this.Valor = valor;
        }
        
        public void Eliminar () {
            this._eliminado = true;
        }

        public int Indice {
            get { return this._indice; }
            private set { this._indice = value; }
        }

        public char Valor {
            get { return this._valor; }
            private set { this._valor = value; }
        }
        
        public bool Eliminado {
            get {return this._eliminado;}
            private set { this._eliminado = value; }
        }
    }

    class Arista
    {
        private int _indice;
        private Vertice _vertice1;
        private Vertice _vertice2;
        private bool _eliminado = false;

        public Arista(int indice, Vertice vertice1, Vertice vertice2)
        {
            this.Indice = indice;
            this.Vertice1 = vertice1;
            this.Vertice2 = vertice2;
        }
        
        public void Eliminar () {
            this._eliminado = true;
        }

        public int Indice {
            get { return this._indice; }
            private set { this._indice = value; }
        }

        public Vertice Vertice1 {
            get { return this._vertice1; }
            private set { this._vertice1 = value; }
        }

        public Vertice Vertice2 {
            get { return this._vertice2; }
            private set { this._vertice2 = value; }
        }
        
        public bool Eliminado {
            get {return this._eliminado;}
            private set { this._eliminado = value; }
        }
    }
}
