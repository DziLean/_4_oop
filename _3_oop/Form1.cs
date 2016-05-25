using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace _3_oop
{
    public partial class MainForm : Form
    {
        public static bool IsValid(string a, string b = "1")
        {
            try
            {
                int age = int.Parse(a);
                int speed = int.Parse(b);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "dll files|*.dll";
            openFileDialog1.Title = "Select a dll";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sFileName = openFileDialog1.FileName;
                var dll = Assembly.LoadFile(sFileName);
                    
                foreach(Type type in dll.GetExportedTypes())
                { 
                    var t = Activator.CreateInstance(type);
                    if (t is Form1)
                    {
                        ListOfForms.Add((Form1)Activator.CreateInstance(type));
                       // classSelection.Items.Add(type);
                    }
                    if (t is Animal)
                    {
                        classSelection.Items.Add(type);
                        types.Add(type);
                    }
                       
                }

            }
              
            
        }


        public static void ClearFields(List<TextBox> LT)
        {
            foreach (TextBox t in LT)
            {
                t.Text = "";
            }
        }

        public List<Animal> ListOfAnimal { get; set; }
        public List<Form1> ListOfForms { get; set; }
        public List<string> Properties { get; set; }
        public List<Type> types;
        public MainForm()
        {
            InitializeComponent();       
            ListOfAnimal = new List<Animal>();
            ListOfForms = new List<Form1>();
            types = new List<Type>();
            types.Add(typeof(Animal));
            types.Add(typeof(Flyers));
            types.Add(typeof(List<Animal>));
        }


        private void _Delete_SelectedIndexChanged(object sender, EventArgs e)
        {
           
           
          
        }

        private void Serialize_Click(object sender, EventArgs e)
        {

            try
            {
                for (int i = 0; i < ListOfAnimal.Count; ++i)
                {
                    _Console.Text += SerializeJson(ListOfAnimal[i])+"!";
                }


                while (ChooseAnimal.Items.Count > 0)
                {
                    ChooseAnimal.Items.RemoveAt(0);
                }
                ListOfAnimal = new List<Animal>();

                /*while (ChooseAnimal.Items.Count > 0)
                {
                    ChooseAnimal.Items.RemoveAt(0);
                }
                ListOfAnimal = new List<Animal>();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deserialization error");
            }
        }

       

        private void Deserialize_Click(object sender, EventArgs e)
        {
            try
            {

                /*Regex r = new Regex("(?<=(\\\"\\$type\\\":\"))[_\\d\\w\\.]{1,}", RegexOptions.IgnoreCase);//:\"[_\\d\\w]{1,}/\\."
                MatchCollection m = r.Matches(_Console.Text);
                Type t = null;
                ListOfAnimal = new List<Animal>();
                List<Type> LocalTypes = new List<Type>();
                
                for (int j=0; j < m.Count; ++j)
                {
                    for (int i = 0; i < types.Count; ++i)
                    {
                        if (m[j].Value == types[i].FullName)
                        {
                            LocalTypes.Add(types[i]);
                            t = types[i];
                            break;
                        }
                    }
                }


              



                Form1 form;
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None, Binder = new binder(LocalTypes.ToArray()) }; //,};
                List<Animal> animal = (List < Animal >) JsonConvert.DeserializeObject(ChangeProp.Text, LocalTypes.GetType());
                for (int i = 0; i < ListOfForms.Count; ++i)
                {
                    string FormName = ListOfForms[i].GetType().Name;
                    FormName = FormName.Substring(0, ListOfForms[i].GetType().Name.Length - 4);
                    if (FormName == animal.GetType().Name)
                    {
                        form = ListOfForms[i];
                        form.BindContent(animal);
                        form.Show();
                        break;
                    }
                }
                ChangeProp.Text = "";
                  */




                Regex r = new Regex("(?<=(\\\"\\$type\\\":\"))[_\\d\\w\\.]{1,}", RegexOptions.IgnoreCase);//:\"[_\\d\\w]{1,}/\\."
                MatchCollection m = r.Matches(_Console.Text);
                Type t = null;
               
                List<Type> LocalTypes = new List<Type>();
                
                for (int j=0; j < m.Count; ++j)
                {
                    for (int i = 0; i < types.Count; ++i)
                    {
                        if (m[j].Value == types[i].FullName)
                        {
                            LocalTypes.Add(types[i]);
                            t = types[i];
                            break;
                        }
                    }
                }




                string desrialize = _Console.Text;
                char[] whitespace = new char[] { '!' };
                string[] str = desrialize.Substring(0,desrialize.Length-1).Split(whitespace);



                _Console.Text = "";
                for (int k = 0; k < LocalTypes.Count; ++k)
                {
                    Animal animal = (Animal)JsonConvert.DeserializeObject(str[k], LocalTypes[k]);
                    ChooseAnimal.Items.Add(animal);
                    ListOfAnimal.Add(animal);
                }

                



            }
            catch (Exception ex)
            {
                MessageBox.Show("Deserialization error");
            }
        }

        private void _delete_Click(object sender, EventArgs e)
        {
            int ind = ChooseAnimal.SelectedIndex;
            ListOfAnimal.RemoveAt(ind);
            ChooseAnimal.Items.RemoveAt(ind);
        }

        private void change_Click(object sender, EventArgs e)
        {
            Form1 form;
            Animal animal = ListOfAnimal[ChooseAnimal.SelectedIndex];
            for (int i = 0; i < ListOfForms.Count; ++i)
            {
                string FormName = ListOfForms[i].GetType().Name;
                FormName = FormName.Substring(0, ListOfForms[i].GetType().Name.Length - 4);
                if (FormName == animal.GetType().Name)
                {
                    form = ListOfForms[i];
                    form.BindContent(animal);
                    form.Show();
                    break;
                }
            }
        }

        private void classSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void create_Click(object sender, EventArgs e)
        {
            Type type = (Type)classSelection.Items[classSelection.SelectedIndex];
            Animal animal = (Animal)Activator.CreateInstance(type);
            ChooseAnimal.Items.Add(animal);
            ListOfAnimal.Add(animal);
            //ListOfAnimal.Add(animal);
            Form1 form;
            for (int i = 0; i < ListOfForms.Count; ++i)
            {
                string FormName = ListOfForms[i].GetType().Name;
                FormName = FormName.Substring(0, ListOfForms[i].GetType().Name.Length - 4);
                if (FormName == type.Name)
                {
                    form = ListOfForms[i];
                    form.BindContent(animal);
                    form.Show();
                    break;
                }
            }
            //form.BindContent(animal);
            

        }
        
        private void ser_Click(object sender, EventArgs e)
        {
          int ind = ChooseAnimal.SelectedIndex;         
          Animal an = ListOfAnimal[ind];
          ChangeProp.Text = SerializeJson(an);
        }

        private void des_Click(object sender, EventArgs e)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None }; //
                Regex r = new Regex("(?<=(\\\"\\$type\\\":\"))[_\\d\\w]{1,}\\.[_\\d\\w]{1,}");//:\"[_\\d\\w]{1,}/\\."
                Match m = r.Match(ChangeProp.Text);
                Type t = null;
                for(int i = 0 ;i< types.Count;++i){
                    if (m.Value == types[i].FullName)
                    {

                        t = types[i];
                        break;
                    }
                }


              



                Form1 form;
                Animal animal = (Animal)JsonConvert.DeserializeObject(ChangeProp.Text, t);
                for (int i = 0; i < ListOfForms.Count; ++i)
                {
                    string FormName = ListOfForms[i].GetType().Name;
                    FormName = FormName.Substring(0, ListOfForms[i].GetType().Name.Length - 4);
                    if (FormName == animal.GetType().Name)
                    {
                        form = ListOfForms[i];
                        form.BindContent(animal);
                        form.Show();
                        break;
                    }
                }
                ChangeProp.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deserialization error");
            }

        }
        public string SerializeJson(Animal an)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };           
            string json = JsonConvert.SerializeObject(an, settings);
            return json;
        }
    


    }

    public class binder : SerializationBinder
    {
        Type[] types;
        public binder(Type[] Types)
        {
            types = Types;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
          
                var type = types.Where(t => t.Name == typeName).FirstOrDefault();

                if (type != null)
                    return type;

            return Type.GetType(typeName + ", " + assemblyName);
        }
    }
}

