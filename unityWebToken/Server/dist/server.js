"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const nunjucks_1 = __importDefault(require("nunjucks"));
// 라우터 가져오기
const lunchRouter_1 = require("./lunchRouter");
const UserRouter_1 = require("./UserRouter");
// 익스프레스 어플리케이션은 웹서버임
let app = (0, express_1.default)();
app.set("view engine", "njk");
nunjucks_1.default.configure("views", { express: app, watch: true });
app.use(express_1.default.json()); // post 로 들어온ㄴ 데이터들을 json 현태로 파싱해주겠다.
app.use(express_1.default.urlencoded({ extended: true }));
// Get, Post, Put, Delete  => Method
// CRUD -> Create, Read, Update, Delete
// App;ication에서 CRUD 를 구현했다 : API
// URI에 get(Read), Post(Create), Put(Update), Delete
// RestFul API (다 구현한 것. 기본이 되는)
// 여기서 라우터 에 set 코드가 있었음
// 점심관련 라우터
app.use(lunchRouter_1.lunchRouter);
// 유저관련 라우터
app.use(UserRouter_1.userRouter);
app.listen(3000, () => {
    console.log(`
        ####################################
        # Server is starting on 3000 port  #
        # http://localhost:3000            #
        ####################################
        `);
});
