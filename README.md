# DigestLoader
Назначение
Система DigestPublish занимается публикацией новостей на сайты info/ и внешний портал

# Схема работы
Ответственный сотрудник присылает набор новостей в формате (тут описание формата) на email. Новости принимаются только от фиксированных отправителей.
Система читает новости их XML-файла и публикует их на:
- интранет-сайте
- интернет-сайте

Опубликованные новости сохраняются в папке SUCCSESS
Если при публикации произошла ошибка (некорректный формат), но новости сохраняются в папку ERROR.

# Схема развертывания
![diagrammaRazvertivaniya](https://github.com/sansansan74/DigestLoader/assets/169544677/23c75caa-65f1-4cf4-8e98-58d2aa5526bf)

# Алгоритм работы
![alg](https://github.com/sansansan74/DigestLoader/assets/169544677/ff37a01f-0610-437a-8bb5-db1b592ce8d3)


# Базы данных
Публикация осуществляется добавление данных о статье в соответствующие БД
Для публикации на интранет сайте info/ - в БД [intranet_sql_server].[site_database] через вызов хранимой процедуры [publish_on_intranet]
Параметры хранимой процедуры:
p1
p2
p3
Для публикации на интернет-портале - в БД [internet_sql_server].[site_database] через вызов хранимой процедуры [publish_on_internet]
