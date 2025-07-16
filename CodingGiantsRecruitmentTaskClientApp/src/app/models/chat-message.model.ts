export interface ChatMessage {
  id: string;
  text: string;
  isFromBot: boolean;
  creationDate: string;
  chatMessageRating?: ChatMessageRating;
}

export interface ChatMessageRating {
  id: string;
  chatMessageId: string;
  chatMessageRatingTypeId: number | null;
  chatMessageRatingType?: ChatMessageRatingType;
  creationDate: string;
}

export interface ChatMessageRatingType {
  id: number;
  name: string;
  icon: string;
}