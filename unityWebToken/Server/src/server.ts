import express, { Application, Request, Response } from "express";                                                                      
import nunjucks from 'nunjucks';

// 미들웨어 가져오기
import { tokenChecker } from "./MyJWT";
// 라우터 가져오기
import {lunchRouter} from "./lunchRouter";
import {userRouter} from "./UserRouter";
import { invenRouter } from "./InventoryRouter";

// 익스프레스 어플리케이션 이건 웹서버임
let app : Application = express();

app.set("view engine", "njk");
nunjucks.configure("views", {express:app, watch:true});

app.use(express.json());    // post 로 들어온ㄴ 데이터들을 json 현태로 파싱해주겠다.
app.use(express.urlencoded({extended:true}));

app.use(tokenChecker);      // 토큰을 체크한다

// Get, Post, Put, Delete  => Method
// CRUD -> Create, Read, Update, Delete
// App;ication에서 CRUD 를 구현했다 : API
// URI에 get(Read), Post(Create), Put(Update), Delete
// RestFul API (다 구현한 것. 기본이 되는)
// 여기서 라우터 에 set 코드가 있었음

app.use(invenRouter);
// 점심관련 라우터
app.use(lunchRouter);
// 유저관련 라우터
app.use(userRouter);

app.listen(3000, () => {
    console.log(
        `
        ####################################
        # Server is starting on 3000 port  #
        # http://localhost:3000            #
        ####################################
        `
    )
});