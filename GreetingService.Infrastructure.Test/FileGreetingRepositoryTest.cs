//using greetingservice.core.entities;
//using greetingservice.infrastructure;
//using greetingservice.core;
//using system;
//using system.collections.generic;
//using system.io;
//using system.linq;
//using system.text.json;
//using xunit;

//namespace greetingservice.infrastructure.test
//{
//    public class filegreetingrepositorytest
//    {
//        public filegreetingrepository _repository { get; set; }

//        private readonly string _filepath;
//        private readonly list<greeting> _testdata;

//        public filegreetingrepositorytest()
//        {
//            _filepath = $"greeting_unit_test_{datetime.now:yyyymmddhhmmss}.json";
//            _repository = new filegreetingrepository(_filepath);

//            _testdata = new list<greeting>
//            {
//                new greeting
//                {
//                    from = "from1",
//                    to = "to1",
//                    message = "message1",
//                },
//                new greeting
//                {
//                    from = "from2",
//                    to = "to2",
//                    message = "message2",
//                },
//                new greeting
//                {
//                    from = "from3",
//                    to = "to3",
//                    message = "message3",
//                },
//                new greeting
//                {
//                    from = "from4",
//                    to = "to4",
//                    message = "message4",
//                },
//            };

//            file.writealltext(_filepath, jsonserializer.serialize(_testdata, new jsonserializeroptions { writeindented = true }));
//        }

//        [fact]
//        public void get_should_return_empty_collection()
//        {
//            var greetings = _repository.getasync();
//            assert.notnull(greetings);
//            assert.notempty(greetings);
//            assert.equal(_testdata.count(), greetings.count());
//        }

//        [fact]
//        public void get_should_return_correct_greeting()
//        {
//            var expectedgreeting1 = _testdata[1];
//            var actualgreeting1 = _repository.getasync(expectedgreeting1.id);
//            assert.notnull(actualgreeting1);
//            assert.equal(expectedgreeting1.id, actualgreeting1.id);

//            var expectedgreeting2 = _testdata[1];
//            var actualgreeting2 = _repository.getasync(expectedgreeting2.id);
//            assert.notnull(actualgreeting2);
//            assert.equal(expectedgreeting2.id, actualgreeting2.id);
//        }

//        [fact]
//        public void post_should_persist_to_file()
//        {
//            var greetingsbeforecreate = _repository.getasync();

//            var newgreeting = new greeting
//            {
//                from = "post_test",
//                to = "post_test",
//                message = "post_test",
//            };

//            _repository.create(newgreeting);

//            var greetingsaftercreate = _repository.getasync();

//            assert.equal(greetingsbeforecreate.count() + 1, greetingsaftercreate.count());
//        }

//        [fact]
//        public void update_should_persist_to_file()
//        {
//            var greetings = _repository.get();

//            var firstgreeting = greetings.first();
//            var firstgreetingmessage = firstgreeting.message;

//            var testmessage = "new updated message";
//            firstgreeting.message = testmessage;

//            _repository.update(firstgreeting);

//            var firstgreetingafterupdate = _repository.get(firstgreeting.id);
//            assert.equal(testmessage, firstgreetingafterupdate.message);
//        }
//    }
//}