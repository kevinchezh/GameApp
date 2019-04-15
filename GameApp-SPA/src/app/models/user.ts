import { Game } from './Game';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    gameUrl: string;
    city: string;
    country: string;
    // ? means this property is optional, only valid after required property
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    games?: Game[];
}
