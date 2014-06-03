using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParser
{
    class SqlVisitor : TSqlFragmentVisitor
    {
        static void Main(string[] args)
        {
            string statement = "insert into Employee2 (Name) select Name from Employee";
            SqlVisitor parser = new SqlVisitor();
            parser.ExtractInfo(statement);
            SendToProxy();
        }

        static void SendToProxy()
        {
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = "localhost,1435";
            cb.InitialCatalog = "alexumtest";
            cb.IntegratedSecurity = false;
            cb.UserID = "alexum@ypphfnm928.database.windows.net";
            cb.Password = "Pa$$word1";
            cb.ConnectTimeout = 600;

            using (SqlConnection con = new SqlConnection(cb.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("test_sp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("select @a + @b", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 600;
                    cmd.Parameters.Add(new SqlParameter("@a", SqlDbType.Int)).Value = 5;
                    cmd.Parameters.Add(new SqlParameter("@b", SqlDbType.Int)).Value = 10;

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void ExtractInfo(string statement)
        {
            TSqlFragment sqlFragment;
            IList<ParseError> errors;
            TSql110Parser parser = new TSql110Parser(true);
            using (StringReader reader = new StringReader(statement))
            {
                sqlFragment = parser.Parse(reader, out errors);
                if (errors != null && errors.Count > 0 )
                {
                    foreach (ParseError error in errors)
                    {
                        Console.WriteLine(error.Message);
                    }
                    return;
                }
            }

            /*IList<TSqlParserToken> tokens = sqlFragment.ScriptTokenStream;
            foreach (TSqlParserToken token in tokens)
            {
                if (token.TokenType == TSqlTokenType.Select)
                {
                    Console.WriteLine(token.TokenType);
                }
                if (token.TokenType == TSqlTokenType.Identifier)
                {
                    Console.WriteLine(token.Text);
                }
            }*/
            sqlFragment.Accept(this);
        }

        public override void Visit(QuerySpecification node)
        {
            foreach (TableReference tableReference in node.FromClause.TableReferences)
            {
                NamedTableReference namedTableReference = tableReference as NamedTableReference;
                if (namedTableReference != null)
                {
                    //Console.WriteLine(namedTableReference.SchemaObject.ToString);
                }
            }
        }

    }
}
