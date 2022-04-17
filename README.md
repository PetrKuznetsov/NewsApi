<p align="center"> # NewsApi </p>
<p align="center">Техническое задание </p>
<p align="center"> Новостной портал: https://www.inform.kz</p>

 ---
 
 <div align="center"> 
 <b>Инструменты: </b>
 
 | NuGet        | Версия         | 
| ------------- |:-------------:| 
| NET Core      | 6.00 LTS      | 
| Auto Mapper   | 11.0.1        |  
| HtmlAgilityPack | 1.12.42     | 
| Nlog          | 4.7.15        | 
| EF Core       | 6.0.4         |
| Swagger       | 6.2.3         |
| JWT Bearer    | 6.0.4         |
| Sql Server    | 6.0.4         |
| Identity model token | 6.17.4 |
</div>

---
<p align="center"><b>Данные для аутентификации в API</b></p>
 
 
 <div align="center">
 
  | Логин        | пароль         |
  | ------------- |:-------------:| 
  | petr      | Qwerty123     | 
 
 </div> 
 
---
<div  align="center">
При запуске приложения, запускается воркер, который отправляет запрос в контроллер для получения новостей с сайта, затем записывает их в базу данных. 
Воркер каждый час парсит данные с сайта, при этом в базу данных записываются только новые записи.
</div>

---
