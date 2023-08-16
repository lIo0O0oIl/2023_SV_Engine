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
exports.recordRouter = void 0;
const express_1 = require("express");
const DB_1 = require("./DB");
const types_1 = require("./types");
exports.recordRouter = (0, express_1.Router)();
// 랭킹 가져오는 것, 데이터 요청
exports.recordRouter.get("/record", (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    const sql = `SELECT u.name, r.* FROM users AS u, ranking AS r WHERE u.id = r.user_id ORDER BY score DESC LIMIT 0,3`;
    // user = u, ranking = r, 로 해서 user의 이름, 랭킹의 모든 것 중에서 유저 아이디와 랭킹 유저 아이디가 같은 것을 가져와 score 기준으로 내림차순이며 상위 3개의 결과를 반환함
    //const sql = `SELECT * FROM ranking ORDER BY score DESC LIMIT 0,3`;
    let [row, col] = yield DB_1.Pool.query(sql);
    let json = { list: row };
    res.json({ type: types_1.MessageType.SUCCESS, message: JSON.stringify(json) });
}));
// 랭킹 데이터를 제출함. 랭킹 변동됨.
exports.recordRouter.post("/record", (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    if (req.user == null) {
        res.json({ type: types_1.MessageType.ERROR, message: "권한이 없습니다." });
        return;
    }
    const user = req.user;
    let { score } = req.body;
    const sql = `INSERT INTO ranking(user_id, score, created) VALUES (?, ?, NOW())`;
    yield DB_1.Pool.execute(sql, [user.id, score]);
    res.json({ type: types_1.MessageType.SUCCESS, message: "기록완료." });
}));
