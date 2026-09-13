namespace TodoApi.Dtos;

public record TodoputDTO(
    string title,
    bool isCompleted
);