import mysql from 'mysql2'

export const dbConfig : mysql.PoolOptions = {
    user: "yy_40114",
    password : "1234",
    database : "yy_40114",
    host: "gondr99.iptime.org",
    port:3306
};

export const JWT_SECRET = "GGM[WORLD_CLASS_OF_HIGHSCHOOL]";