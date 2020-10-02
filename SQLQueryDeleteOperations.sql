DELETE FROM OutgoingMessages;
dbcc checkident (OutgoingMessages, reseed, 0);