import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChatMessage, ChatMessageRatingType } from '../../models/chat-message.model';
import { ChatService } from '../../services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatTooltipModule
  ]
})
export class ChatComponent implements OnInit {
    
  messages: ChatMessage[] = [];
  ratingTypes: ChatMessageRatingType[] = [];
  userInput: string = '';
  botMessage?: ChatMessage;
  loading: boolean = false;
  errorMessage: string = '';
  @ViewChild('messagesContainer') messagesContainer!: ElementRef<HTMLDivElement>;

  constructor(private chatService: ChatService) {}

  ngOnInit() {
    this.chatService.startHubConnection().then(() => {
      this.chatService.listenForMessageId((id) => { 
        if (this.botMessage)
          this.botMessage.id = id;
      });
      this.chatService.listenForMessageFragments(fragmentText => {
        if (this.botMessage)
          this.botMessage.text = fragmentText;

        this.scrollToBottom();
      });
      this.chatService.listenForError((message) => {
        this.errorMessage = message;
        this.scrollToBottom();
      });
    });

    this.chatService.getChatHistory().subscribe(history => {
      this.messages = history;
      this.scrollToBottom();
    });
    this.chatService.getRatingTypes().subscribe(ratingTypes => this.ratingTypes = ratingTypes);
  }

  scrollToBottom() {
    setTimeout(() => {
      this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
    }, 0);
  }

  sendMessage() {
    if (!this.userInput) 
      return;

    this.loading = true;
    var creationDate = new Date().toISOString();

    this.messages.push({
      id: '',
      text: this.userInput,
      isFromBot: false,
      creationDate: creationDate
    });

    this.botMessage = {
      id: '',
      text: '',
      isFromBot: true,
      creationDate: creationDate
    };

    this.chatService.startGenerating(this.userInput).then(() => {
      this.onGenerationComplete();
    });
    this.userInput = '';
  }

  cancelMessage() {
    if (this.botMessage && this.botMessage.id !== '') {
      this.chatService.cancelGenerating(this.botMessage.id).subscribe(() => {
          this.onGenerationComplete();
      });
    }
  }

  onGenerationComplete() {
    if (this.botMessage) {
      this.messages.push(this.botMessage);
      this.botMessage = undefined;
      this.loading = false;
    }
  }

  rateMessage(messageId: string, ratingTypeId: number) {
    const message = this.messages.find(m => m.id === messageId);
    const newRating = message?.chatMessageRating?.chatMessageRatingTypeId === ratingTypeId ? null : ratingTypeId;

    this.chatService.rateMessage(messageId, newRating).subscribe(() => {
      console.log(`Rated message ${messageId} with rate type Id: ${ratingTypeId}`);

      if (message) {
        message.chatMessageRating = {
          id: '',
          chatMessageId: messageId,
          chatMessageRatingTypeId: newRating,
          creationDate: new Date().toISOString(),
        }
      }
    });
  }

  isRateSelected(message: ChatMessage, ratingType: ChatMessageRatingType): boolean {
    return message.chatMessageRating?.chatMessageRatingTypeId === ratingType.id;
  }
}