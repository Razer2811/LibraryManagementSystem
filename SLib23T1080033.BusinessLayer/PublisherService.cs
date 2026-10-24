using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.BusinessLayer;

public class PublisherService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository
            ?? throw new ArgumentNullException(nameof(publisherRepository));
    }

    // Lấy danh sách nhà xuất bản
    public async Task<IEnumerable<Publisher>> ListAsync()
    {
        return await _publisherRepository.ListAsync();
    }

    // Lấy nhà xuất bản theo ID
    public async Task<Publisher> GetAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID nhà xuất bản không hợp lệ.");

        var publisher = await _publisherRepository.GetAsync(id);

        if (publisher == null)
            throw new Exception("Không tìm thấy nhà xuất bản.");

        return publisher;
    }

    // Thêm nhà xuất bản
    public async Task<int> AddAsync(Publisher publisher)
    {
        ValidatePublisher(publisher);

        publisher.PublisherName = publisher.PublisherName.Trim();

        return await _publisherRepository.AddAsync(publisher);
    }

    // Cập nhật nhà xuất bản
    public async Task<bool> UpdateAsync(Publisher publisher)
    {
        ValidatePublisher(publisher);

        if (publisher.PublisherId <= 0)
            throw new Exception("ID nhà xuất bản không hợp lệ.");

        var existingPublisher =
            await _publisherRepository.GetAsync(publisher.PublisherId);

        if (existingPublisher == null)
            throw new Exception("Không tìm thấy nhà xuất bản cần cập nhật.");

        publisher.PublisherName = publisher.PublisherName.Trim();

        var affectedRows =
            await _publisherRepository.UpdateAsync(publisher);

        return affectedRows > 0;
    }

    // Xóa nhà xuất bản
    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID nhà xuất bản không hợp lệ.");

        var publisher = await _publisherRepository.GetAsync(id);

        if (publisher == null)
            throw new Exception("Không tìm thấy nhà xuất bản cần xóa.");

        if (await _publisherRepository.InUsedAsync(id))
            throw new Exception(
                "Không thể xóa nhà xuất bản vì đang được sử dụng.");

        var affectedRows = await _publisherRepository.DeleteAsync(id);

        return affectedRows > 0;
    }

    // Kiểm tra nhà xuất bản đang được sử dụng
    public async Task<bool> InUsedAsync(int id)
    {
        if (id <= 0)
            throw new Exception("ID nhà xuất bản không hợp lệ.");

        var publisher = await _publisherRepository.GetAsync(id);

        if (publisher == null)
            throw new Exception("Không tìm thấy nhà xuất bản.");

        return await _publisherRepository.InUsedAsync(id);
    }

    // Kiểm tra dữ liệu nhà xuất bản
    private void ValidatePublisher(Publisher publisher)
    {
        if (publisher == null)
            throw new Exception(
                "Thông tin nhà xuất bản không được để trống.");

        if (string.IsNullOrWhiteSpace(publisher.PublisherName))
            throw new Exception(
                "Tên nhà xuất bản không được để trống.");

        if (publisher.PublisherName.Trim().Length > 200)
            throw new Exception(
                "Tên nhà xuất bản không được vượt quá 200 ký tự.");
    }
}