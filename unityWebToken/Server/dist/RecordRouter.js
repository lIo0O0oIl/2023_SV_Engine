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
exports.recordRouter.get("/record", (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    const sql = `SELECT u.name, r.* FROM users AS u, ranking AS r WHERE u.id = r.user_id ORDER BY score DESC LIMIT 0,3`;
    //const sql = `SELECT * FROM ranking ORDER BY score DESC LIMIT 0,3`;
    let [row, col] = yield DB_1.Pool.query(sql);
    let json = { list: row };
    res.json({ type: types_1.MessageType.SUCCESS, message: JSON.stringify(json) });
}));
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
