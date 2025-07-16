import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ChatMessage, ChatMessageRatingType } from '../models/chat-message.model';

@Injectable({ providedIn: 'root' })
export class ChatService {

  private readonly baseUrl = environment.apiUrl;
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient) {}

  startHubConnection(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/hubs/chatHub`)
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start();
  }

  listenForMessageId(callback: (id: string) => void): void {
    this.hubConnection.on('ReceiveMessageId', (data: { id: string }) => callback(data.id));
  }

  listenForMessageFragments(callback: (fragment: string) => void): void {
    this.hubConnection.on('ReceiveMessageFragment', (data: { fragmentText: string }) => callback(data.fragmentText));
  }

  listenForError(callback: (message: string) => void): void {
    this.hubConnection.on('Error', (data: { message: string }) => callback(data.message));
  }

  startGenerating(userMessage: string): Promise<void> {
    return this.hubConnection.invoke('StartGenerating', userMessage);
  }

  cancelGenerating(messageId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/chat/cancelGenerating/${messageId}`);
  }

  getChatHistory(): Observable<ChatMessage[]> {
    return this.http.get<ChatMessage[]>(`${this.baseUrl}/api/chat/history`);
  }

  getRatingTypes(): Observable<ChatMessageRatingType[]> {
    return this.http.get<ChatMessageRatingType[]>(`${this.baseUrl}/api/chat/messageRatingTypes`);
  }

  rateMessage(messageId: string, ratingTypeId: number | null): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/api/chat/rate`, {
      chatMessageId: messageId,
      chatMessageRatingTypeId: ratingTypeId
    });
  }
}