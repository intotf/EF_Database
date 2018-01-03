# EF_Database
  C# 通过 EF CodeFirst 连接各类DB 小实例<br>
  将路续完善与 Mysql,Mongodb,Redis,Access,SqlLite 等数据库的连接；

## 完成 与 Mssql Server 的数据读写操作
  Sqlserver 使用 2012 版本；(已放置外网服务器，请误修改)<br>
  Web 使用 MVC5，采用异步读写方式；<br>
  WinFrom 使用异步读写方式；<br>
  
## 完成 与 Mysql 的数据读写操作
  Mysql 使用 5.x 版本；(已放置外网服务器，请误修改)<br>
  Web 使用 MVC5，采用异步读写方式；<br>
  WinFrom 使用异步读写方式；<br>
  MySql.Data 和 MySql.Data.Entity.EF6 建议使用 6.9.9.0,其它版本有些坑位暂不做解释<br>
  
## 完成 与 Mongodb 的数据读写操作
  Mongodb 使用 3.2 版本；(请自行架设)<br>
  Web 使用 MVC5，采用异步读写方式；<br>
  WinFrom 使用异步读写方式，可按年、月进行分表存储；<br>
  
## 完成 与 Redis 的数据读写操作
  Redis 使用 3.0.501 版本；(请自行架设)<br>
  采用异步读写方式，实现增加订阅者，删除所有订阅者，创建数据后可通知到WinForm<br>
  WinFrom 使用异步读写，消息异步通知回调处理；<br>
  
