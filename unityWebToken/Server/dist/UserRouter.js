"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.userRouter = void 0;
const express_1 = require("express");
const DB_1 = require("./DB");
const types_1 = require("./types");
exports.userRouter = (0, express_1.Router)();
exports.userRouter.get("/user/login", (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    // 이건 구현 안할꺼임
}));
exports.userRouter.post("/user/login", (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    let { email, password } = req.body;
    console.log(email, password);
}));
exports.userRouter.get("/user/register", (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    res.render("register");
}));
exports.userRouter.post("/user/register", (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    console.log(req.body);
    let email = req.body.email;
    let password = req.body.password;
    let passwordc = req.body.passwordc;
    let username = req.body.username;
    if (email == "" || password == "" || username == "") {
        let mag = { type: types_1.MessageType.ERROR, message: "필수값이 비어있습니다" };
        res.json(mag);
        return;
    }
    // email(20*1), name(4*3) => 32바이트
    if (password != passwordc) {
        let mag = { type: types_1.MessageType.ERROR, message: "비밀번호와 확인이 일치하지 않습니다." };
        res.json(mag);
        return;
    }
    const sql = "INSERT INTO users (email, password, name) VALUES (?, PASSWORD(?), ?)";
    let [result, info] = yield DB_1.Pool.execute(sql, [email, password, username]);
    if (result.affectedRows != 1) {
        let mag = { type: types_1.MessageType.ERROR, message: "DB에 이상이 있어. ㅁ문의해" };
        res.json(mag);
        return;
    }
    // 데이터 베이스에 값 저장하기
    let mag = { type: types_1.MessageType.SUCCESS, message: "로그인성공 ㅇ" };
    res.json(mag);
}));
