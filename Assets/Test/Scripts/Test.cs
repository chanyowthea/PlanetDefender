using UnityEngine;
using System.Collections;
using System.Text;

public class Test : MonoBehaviour
{
    private Sqlite m_sqlite;
    private const string TABLE_NAME = "UnityTest";
    // Use this for initialization
    void Start()
    {
        //数据库地址
        string dbPath = Application.dataPath + "/test.db";
        m_sqlite = new Sqlite(dbPath);

        //创建新表
        CreateTable();

        //插入数据
        TestConfig tc1 = new TestConfig
        {
            m_id = "001",
            m_name = "test1",
            m_len = 10
        };
        InsertTable(tc1);

        //在插入一条
        TestConfig tc2 = new TestConfig
        {
            m_id = "002",
            m_name = "test2",
            m_len = 20
        };
        InsertTable(tc2);

        //查询数据
        SelectTable();

        //更新数据
        tc2.m_name = "johnny";
        tc2.m_len = 5;
        UpdateTable(tc2);

        SelectTable();
    }

    void OnDestroy()
    {
        m_sqlite.Close();
    }

    private void CreateTable()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("create table ");
        sql.Append(TABLE_NAME);
        sql.Append("(ID VARCHAR(255) PRIMARY KEY,");
        sql.Append("Name VARCHAR(255),");
        sql.Append("Len INT);");
        Excute(sql.ToString());
    }

    private void InsertTable(TestConfig tc)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("insert into ");
        sql.Append(TABLE_NAME);
        sql.Append(" values ('");
        sql.Append(tc.m_id);
        sql.Append("','");
        sql.Append(tc.m_name);
        sql.Append("',");
        sql.Append(tc.m_len);
        sql.Append(");");
        Excute(sql.ToString());
    }

    private void SelectTable()
    {
        string sql = "select * from " + TABLE_NAME;
        foreach (var tc in m_sqlite.ExcuteSelectQuery<TestConfig>(sql))
        {
            Debug.Log(tc);
        }
    }

    private void UpdateTable(TestConfig tc)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("update ");
        sql.Append(TABLE_NAME);
        sql.Append(" set Name = '");
        sql.Append(tc.m_name);
        sql.Append("', Len = ");
        sql.Append(tc.m_len);
        sql.Append(" where ID = '");
        sql.Append(tc.m_id);
        sql.Append("';");
        Excute(sql.ToString());
    }

    private void Excute(string sql)
    {
        Debug.Log(sql.ToString());
        m_sqlite.ExcuteQuery(sql);
    }
}