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
exports.invenRouter = void 0;
const express_1 = require("express");
const DB_1 = require("./DB");
const types_1 = require("./types");
exports.invenRouter = (0, express_1.Router)();
exports.invenRouter.get("/inven", (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    if (req.user == null) {
        res.json({ type: types_1.MessageType.ERROR, message: "권한이 없습니다." });
        return;
    }
    const user = req.user;
    let sql = "SELECT json FROM inventories WHERE user_id = ?";
    let [rows, col] = yield DB_1.Pool.query(sql, [user.id]);
    if (rows.length == 0) {
        res.json({ type: types_1.MessageType.EMPTY, message: "" });
    }
    else {
        let json = rows[0].json;
        res.json({ type: types_1.MessageType.SUCCESS, message: json });
    }
}));
exports.invenRouter.post("/inven", (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    if (req.user == null) {
        res.json({ type: types_1.MessageType.ERROR, message: "권한이 없습니다." });
        return;
    }
    const user = req.user;
    let json = req.body.json;
    let vo = JSON.parse(json);
    //let vo : InventoryVO = req.body;        // 유니티 버전이 2022 라서
    const sql = `INSERT INTO inventories (user_id, json) VALUES (?, ?) 
                   ON DUPLICATE KEY UPDATE json = VALUES(json)`;
    let [result, info] = yield DB_1.Pool.execute(sql, [user.id, JSON.stringify(vo)]);
    //result.affectedRows 값이 2면 기존에 존재해서 업데이트 했어, 1이면 없어서 insert했어 
    let msg = { type: types_1.MessageType.SUCCESS, message: "인벤토리 저장완료" };
    res.json(msg);
}));
