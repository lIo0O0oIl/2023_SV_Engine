"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Pool = void 0;
const Secret_1 = require("./Secret");
const promise_1 = __importDefault(require("mysql2/promise"));
exports.Pool = promise_1.default.createPool(Secret_1.dbConfig);
