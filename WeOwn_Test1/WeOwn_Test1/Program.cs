using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;   // NECESSARY FOR A MySQL SERVER AND DATABASE
using System.Data;              //NECESSARY FOR MANIPULATING DATASETS

namespace WeOwn_Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            MySqlConnection conn = null;        // DECLARING THE CONNECTION NECESSARY TO CONNECT TO THE SERVER AND DATABASE

            string connstring = @"server=192.168.100.131;" +        // DECLARING THE CONNECTION STRING AND FILLING IT WITH
                                "userid=mesic;" +                   // THE NECESSARY VALUES FOR IDENTIFICATION
                                "password=0603401417Aa;" +           // server - to which we are connecting      |       userid - name of a user
                                "database=combo_of_3;AllowLoadLocalInfile=true";                   // password - password of the user          |       database - name of the database the user wants to query
                                                                                                   // AllowLoadLocalInfile - try to save something to a local machine without this, I dare you

            string start_date = null, end_date = null;

            int airline_id = 0;

            start_date = "2018-01-01";           // THE VARIABLES THE USER CAN PASS TO THE PROGRAM TO IDENTIFY NEW and DISCONTINUED FLIGHTS
            end_date = "2018-01-15";             // THEY ARE INITIALIZED BEFOREHAND SO THE USER DOESN't HAVE TO
            airline_id = 33;

            if (args[0] != null)
                start_date = args[0];
            if (args[1] != null)
                end_date = args[1];
            if (args[2] != null)
                airline_id = Int16.Parse(args[2]);

            if (start_date == null && end_date == null && airline_id == 0)          // I COULD DO THIS WITH A SWITCH FUNCTION, IT WOULD HAVE BEEN EASIER,
                                                                                    // BUT I CHOSE NESTED IF's, BAD CHOICE
            {
                watch.Stop();
                Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms. \n");
                Console.WriteLine("Please enter the correct date format: YYYY-MM-DD, as well as the correct airline ID.");
                Console.WriteLine("An example would be: Program.exe 2018-01-01 2018-01-15 1");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
            else
            {
                if (start_date == null)
                {
                    watch.Stop();
                    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms. \n");
                    Console.WriteLine("Please enter the correct date format: YYYY-MM-DD, as well as the correct airline ID.");
                    Console.WriteLine("An example would be: Program.exe 2018-01-01 2018-01-15 1");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey(true);
                    Environment.Exit(0);

                }
                else if (end_date == null)
                {
                    watch.Stop();
                    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms. \n");
                    Console.WriteLine("Please enter the correct date format: YYYY-MM-DD, as well as the correct airline ID.");
                    Console.WriteLine("An example would be: Program.exe 2018-01-01 2018-01-15 1");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
                else if (airline_id == 0)
                {
                    watch.Stop();
                    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms. \n");
                    Console.WriteLine("Please enter the correct date format: YYYY-MM-DD, as well as the correct airline ID.");
                    Console.WriteLine("An example would be: Program.exe 2018-01-01 2018-01-15 1");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Processing....");

                    try
                    {
                        conn = new MySqlConnection(connstring);     // FILLING THE CONNECTION WITH THE NECESSARY VALUES
                        conn.Open();                                // OPENING A CONNECTION TO THE SERVER

                        /*
                         * 
                              string start_date = "2018-01-01";           // THE VARIABLES THE USER CAN PASS TO THE PROGRAM TO IDENTIFY NEW and DISCONTINUED FLIGHTS
                              string end_date = "2018-01-15";             // THEY ARE INITIALIZED BEFOREHAND SO THE USER DOESN't HAVE TO
                              int airline_id = 33;

                         * 
                         */


                        /*
                         * 
                         * if (args.ElementAt(0) != null)
                            start_date = args.ElementAt(0);
                        if (args.ElementAt(1) != null)
                            end_date = args.ElementAt(1);
                        if (args.ElementAt(2) != null)
                            airline_id = Int16.Parse(args.ElementAt(2));
                         * 
                         */




                        // QUERY TO EXPORT EVERYTHING INTO A CSV FILE WITH THE NECESSARY HEADERS

                        // THE "UNION" STRING OPERATOR WAS USED TO CREATE THE HEADERS FOR TEH COLUMNS

                        string exportToCSVQuery = $"(SELECT 'flight_id','route_id','departure_time', 'arrival_time', 'airline_id', 'Status')UNION SELECT flight_id, route_id, departure_time, arrival_time, airline_id, 'New Flight' FROM flights WHERE airline_id = {airline_id} AND departure_time > DATE_SUB('{start_date}', INTERVAL 30 MINUTE) AND departure_time < DATE_ADD('{end_date}', INTERVAL 30 MINUTE) INTO OUTFILE 'E:\\WeOwn_CSV/results.csv' COLUMNS TERMINATED BY ',' LINES TERMINATED BY '\\n';";



                        System.IO.File.Delete(@"E:\WeOwn_CSV/results.csv");         // DELETING THE FILE WE WANT TO STORE WITH THE EXPORT QUERY



                        MySqlDataAdapter da = new MySqlDataAdapter(exportToCSVQuery, conn);     // DECLARING A DATA ADAPTER FOR MySQL TO MANIPULATE THE TABLES OF A DATABASE
                        DataSet ds = new DataSet();                                             // AS WELL AS A DATASET TO MANIPULATE AND COMPLETE THE QUERY
                        da.Fill(ds, "flights");


                        //DataTable dt = ds.Tables["flights"];          // NO LONGER NECESARRY SINCE I AM DOING EVERYTHING WITH A QUERY AND NOT USING THE VISUAL STUDIO's
                        // OWN DATA MANIPULATION METHODS
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: {0}", e.ToString());      //WRITING AN ERROR STRING INTO THE CONSOLE WHEN SOMETHING WENT WRONG
                        conn.Close();
                        watch.Stop();
                        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            conn.Close();               //CLOSING THE CONNECTION TO THE SERVER
                            watch.Stop();

                            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms. \n");       // WRITING TO THE CONSOLE HOW MUCH TIME HAS ELAPSED SINCE THE START
                                                                                                            // OF THE PROGRAM


                            Console.WriteLine("The file path of the overwritten copy is: 'E:\\Results/results.csv'! \n");
                            Console.WriteLine("Press any key to close this window.");
                            Console.ReadKey(true);      //TRY AND READ THE STATEMENT INSIDE THE CONSOLE WITHOUT THIS, I DARE YOU

                            System.IO.File.Copy(@"E:\WeOwn_CSV/results.csv", @"E:\Results/results.csv", true);
                        }
                    }
                }
            }

            /*********************** 
             * 
             * A BUNCH OF QUERY's TO APPLY TO THE "MySqlCommand command = new MySqlCommand();" QUERY 
             * 
             * ***********************

            string createDB = "CREATE A DATABASE IF NOT EXISTS combo_of_3;";          

            string useDB = "USE combo_of_3;";

            string createTableSub = "CREATE TABLE IF NOT EXISTS subscriptions(" +
                "agency_id INT NOT NULL," +
                "origin_city_id INT NOT NULL," +
                "destination_city_id INT NOT NULL);";

            string createTableRoute = "CREATE TABLE IF NOT EXISTS routes(" +
                "route_id INT NOT NULL," +
                "origin_city_id INT NOT NULL," +
                "destination_city_id INT NOT NULL," +
                "departure_date DATE NOT NULL," +
                "PRIMARY KEY (route_ID));";

            string createTableFlight = "CREATE TABLE IF NOT EXISTS flights(" +
                "flight_id INT NOT NULL," +
                "route_id INT NOT NULL," +
                "departure_time DATETIME NOT NULL," +
                "arrival_time DATETIME NOT NULL," +
                "airline_id INT NOT NULL," +
                "PRIMARY KEY (flight_id)," +
                "FOREIGN KEY (route_id) REFERENCES routes(route_id));";

            string loadDataSub = "LOAD DATA LOCAL INFILE 'E:\\WeOwn_CSV / subscriptions.csv' INTO TABLE subscriptions" +
                  "COLUMNS TERMINATED BY ','" +
                  "LINES TERMINATED BY '\n'" +
                  "IGNORE 1 LINES;";

            string loadDataRoute = "LOAD DATA LOCAL INFILE 'E:\\WeOwn_CSV / routes.csv' INTO TABLE routes" +
                  "COLUMNS TERMINATED BY ','" +
                  "LINES TERMINATED BY '\n'" +
                  "IGNORE 1 LINES;";

            string loadDataFlight = "LOAD DATA LOCAL INFILE 'E:\\WeOwn_CSV / flights.csv' INTO TABLE flights" +
                  "COLUMNS TERMINATED BY ','" +
                  "LINES TERMINATED BY '\n'" +
                  "IGNORE 1 LINES;";


            
            string query = $"SELECT * FROM flights WHERE airline_id = {airline_id} AND departure_time > DATE_SUB('{start_date}', INTERVAL 30 MINUTE) AND departure_time < DATE_ADD('{end_date}', INTERVAL 30 MINUTE);";
                 

            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandText = query;
            command.ExecuteNonQuery();

            //MySqlCommand command = new MySqlCommand("LOAD DATA LOCAL INFILE 'E:\\WeOwn_CSV/subscriptions.csv' INTO TABLE subscription123 COLUMNS TERMINATED BY ',' LINES TERMINATED BY '\n' IGNORE 1 LINES;", conn);


            ******************************************************************************************************************************************************************************************************************************/

            /*
             * 
             * DataRow[] dr = dt.Select("airline_id = 33");
            foreach (DataRow row in dr)
            {
                var name = row["departure_time"].ToString();
                var contact = row["arrival_time"].ToString();

                Console.WriteLine(name + " | " + contact);
            }
             * 
             */


            /*
             * 
             * foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    Console.Write(row[col] + "\t");             //A SIMPLE LOOP WHICH READS THE VALUES FROM A TABLE AND PUTS THEM IN A CONSOLE
                }

                Console.Write("\n");        //NEXT ROW
            }
             */
        }
    }
}
