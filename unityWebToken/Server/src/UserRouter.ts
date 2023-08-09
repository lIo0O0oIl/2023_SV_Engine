import { Router, Request, Response } from "express";                                                                      
import {Pool} from "./DB";
import {RowDataPacket, ResultSetHeader} from 'mysql2/promise'
import { MessageType, ResponseMSG } from "./types";

export const userRouter = Router();

userRouter.get("/user/login", async (req:Request, res:Response) => {

});

userRouter.post("/user/login", async (req:Request, res:Response) => {

});

userRouter.get("/user/register", async (req:Request, res:Response) => {
    res.render("register");
});

userRouter.post("/user/register", async (req:Request, res:Response) => {
    console.log(req.body);
    let email : string = req.body.email;
    let password : string = req.body.password;
    let passwordc : string = req.body.passwordc;
    let username : string = req.body.username;

    if (email == "" || password == "" || username == ""){                                           
        let mag:ResponseMSG = {type: MessageType.ERROR, message : "필수값이 비어있습니다"};
        res.json(mag);
        return;
    }
    // email(20*1), name(4*3) => 32바이트
    if (password != passwordc){
        let mag:ResponseMSG = {type: MessageType.ERROR, message : "비밀번호와 확인이 일치하지 않습니다."};
        res.json(mag);
        return;
    }

    const sql : string = "INSERT INTO users (email, password, name) VALUES (?, PASSWORD(?), ?)";
    let [result, info] : [ResultSetHeader, any] = await Pool.execute(sql, [email, password, username]);
    
    if (result.affectedRows != 1){
        let mag : ResponseMSG = {type:MessageType.ERROR, message: "DB에 이상이 있어. ㅁ문의해"};
        res.json(mag);
        return;
    }

    // 데이터 베이스에 값 저장하기

        let mag : ResponseMSG = {type:MessageType.SUCCESS, message: "로그인성공 ㅇ"};
        res.json(mag);
});