namespace TodoApi.Dtos;

public record TodoGetDto(
    int id,
    string title,
    bool isCompleted
);